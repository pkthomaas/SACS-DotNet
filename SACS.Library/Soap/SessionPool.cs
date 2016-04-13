using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using SACS.Library.SabreSoapApi;

namespace SACS.Library.Soap
{
    /// <summary>
    /// The session pool.
    /// </summary>
    public class SessionPool : ISessionPool
    {
        /// <summary>
        /// The log
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(SessionPool));

        /// <summary>
        /// The mapping between conversation identifiers and busy sessions.
        /// </summary>
        private readonly ConcurrentDictionary<string, Security> busy = new ConcurrentDictionary<string, Security>();

        /// <summary>
        /// The queue of available sessions.
        /// </summary>
        private readonly ConcurrentQueue<Security> availableQueue = new ConcurrentQueue<Security>();

        /// <summary>
        /// The session count.
        /// </summary>
        private readonly int count;

        /// <summary>
        /// The semaphore used for synchronizing multi-threaded access.
        /// </summary>
        private readonly SemaphoreSlim semaphore;

        /// <summary>
        /// The SOAP authentication service.
        /// </summary>
        private readonly SoapAuth soapAuth;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionPool"/> class.
        /// </summary>
        /// <param name="soapAuth">The SOAP authentication service.</param>
        /// <param name="count">The count.</param>
        public SessionPool(SoapAuth soapAuth, int count)
        {
            this.soapAuth = soapAuth;
            this.count = count;
            this.semaphore = new SemaphoreSlim(0, count);
        }

        /// <summary>
        /// Populates the session pool asynchronously, creating new sessions.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task PopulateAsync()
        {
            int successes = 0;
            for (int i = 0; i < this.count; ++i)
            {
                try
                {
                    string conversationId = "AuthConversation_" + i;
                    Log.Debug("Started auth request.");
                    if (await this.CreateSessionAsync(conversationId))
                    {
                        successes += 1;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Auth request failed", ex);
                }
            }

            this.semaphore.Release(successes);
        }

        /// <summary>
        /// Takes the security token from the session pool asynchronously.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        /// <returns>
        /// The security token.
        /// </returns>
        public async Task<Security> TakeSessionAsync(string conversationId)
        {
            if (!this.busy.ContainsKey(conversationId))
            {
                Log.DebugFormat("ConversationId: {0} waits for semaphore (count before = {1})", conversationId, this.semaphore.CurrentCount);
                await this.semaphore.WaitAsync();
                Log.DebugFormat("ConversationId: {0} acquired semaphore (count after = {1})", conversationId, this.semaphore.CurrentCount);
                try
                {
                    Security security;
                    bool succeeded = this.availableQueue.TryDequeue(out security);
                    if (succeeded)
                    {
                        this.busy[conversationId] = security;
                    }
                    else
                    {
                        Log.DebugFormat("ConversationId: {0} failed to get from queue! (count = {1})", conversationId, this.semaphore.CurrentCount);
                        this.semaphore.Release();
                    }
                }
                catch (Exception)
                {
                    this.semaphore.Release();
                }
            }

            return this.busy[conversationId];
        }

        /// <summary>
        /// Releases the session.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Can't find session for this ConversationID.</exception>
        public void ReleaseSession(string conversationId)
        {
            Security security;
            bool removed = this.busy.TryRemove(conversationId, out security);
            if (removed)
            {
                Log.DebugFormat("ConversationId: {0} releases semaphore (count before = {1})", conversationId, this.semaphore.CurrentCount);
                this.availableQueue.Enqueue(security);
                this.semaphore.Release();
            }
            else
            {
                Log.DebugFormat("ConversationId: {0} can't find session to release", conversationId);
                throw new KeyNotFoundException("Can't find session for this ConversationID.");
            }
        }

        /// <summary>
        /// Refreshes the session pool asynchronously, renewing the sessions.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task RefreshAsync()
        {
            bool canContinue = true;
            List<Security> refreshed = new List<Security>();
            List<Security> failed = new List<Security>();
            int i = 0;
            while (canContinue && this.semaphore.CurrentCount > 0)
            {
                await this.semaphore.WaitAsync();
                Security security = null;
                try
                {
                    canContinue = this.availableQueue.TryDequeue(out security);
                    if (canContinue)
                    {
                        var result = await this.soapAuth.TryRefreshAsync(security, "RefreshConversation_" + i);
                        i += 1;
                        if (result.IsOk)
                        {
                            Log.Debug("Refreshed session.");
                            refreshed.Add(security);
                        }
                        else
                        {
                            Log.DebugFormat("Failed to refresh session: {0}", result.ErrorType.Value);
                            failed.Add(security);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat("Error during refreshing session", ex);
                    if (security == null)
                    {
                        failed.Add(security);
                    }
                }
            }

            refreshed.ForEach(this.availableQueue.Enqueue);
            this.semaphore.Release(refreshed.Count);

            foreach (int j in failed.Select((o, j) => j))
            {
                if (await this.CreateSessionAsync("AuthConversation_" + j))
                {
                    this.semaphore.Release();
                }
            }
        }

        /// <summary>
        /// Creates the session asynchronously and adds it to the available queue if possible.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        /// <returns><c>true</c> if succeeded; otherwise <c>false</c></returns>
        private async Task<bool> CreateSessionAsync(string conversationId)
        {
            var authResult = await this.soapAuth.CreateSessionAsync(conversationId);
            if (authResult.IsOk)
            {
                Log.Debug("Auth request OK.");
                this.availableQueue.Enqueue(authResult.Security);
                return true;
            }
            else if (authResult.Exception != null)
            {
                Log.Error("Error when creating new session", authResult.Exception);
            }
            else
            {
                Log.Debug("Auth request failed");
            }

            return false;
        }
    }
}