using System.Collections.Concurrent;
using System.Threading.Tasks;
using log4net;
using SACS.Library.SabreSoapApi;

namespace SACS.Library.Soap
{
    /// <summary>
    /// The simple session pool that only creates new session and releases them.
    /// </summary>
    public class SessionPoolSimple : ISessionPool
    {
        /// <summary>
        /// The log
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(SessionPoolSimple));

        /// <summary>
        /// The dictionary containing mappings between conversation identifiers and security tokens.
        /// </summary>
        private readonly ConcurrentDictionary<string, Security> busy = new ConcurrentDictionary<string, Security>();

        /// <summary>
        /// The SOAP authentication service.
        /// </summary>
        private readonly SoapAuth soapAuth;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionPoolSimple"/> class.
        /// </summary>
        /// <param name="soapAuth">The SOAP authentication service.</param>
        public SessionPoolSimple(SoapAuth soapAuth)
        {
            this.soapAuth = soapAuth;
        }

        /// <summary>
        /// Populates the session pool asynchronously, creating new sessions.
        /// Does nothing in this implementation.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task PopulateAsync()
        {
            await Task.Yield();
        }

        /// <summary>
        /// Refreshes the session pool asynchronously, renewing the sessions.
        /// Does nothing in this implementation.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task RefreshAsync()
        {
            await Task.Yield();
        }

        /// <summary>
        /// Releases the session.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        public void ReleaseSession(string conversationId)
        {
            Security security;
            this.busy.TryRemove(conversationId, out security);
        }

        /// <summary>
        /// Takes the security token from the session pool asynchronously.
        /// In this implementation, just creates a new session.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        /// <returns>
        /// The security token.
        /// </returns>
        public async Task<Security> TakeSessionAsync(string conversationId)
        {
            Security security = null;
            if (!this.busy.TryGetValue(conversationId, out security))
            {
                var authResult = await this.soapAuth.CreateSessionAsync(conversationId);
                if (authResult.IsOk)
                {
                    Log.Debug("Auth request OK.");
                    security = authResult.Security;
                    this.busy[conversationId] = security;
                }
                else if (authResult.Exception != null)
                {
                    Log.Error("Error when creating new session", authResult.Exception);
                }
                else
                {
                    Log.Debug("Auth request failed");
                }
            }

            return security;
        }
    }
}