using System.Threading.Tasks;
using SACS.Library.SabreSoapApi;

namespace SACS.Library.Soap
{
    /// <summary>
    /// The SOAP session pool.
    /// </summary>
    public interface ISessionPool
    {
        /// <summary>
        /// Populates the session pool asynchronously, creating new sessions.
        /// </summary>
        /// <returns>The task.</returns>
        Task PopulateAsync();

        /// <summary>
        /// Refreshes the session pool asynchronously, renewing the sessions.
        /// </summary>
        /// <returns>The task.</returns>
        Task RefreshAsync();

        /// <summary>
        /// Releases the session.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        void ReleaseSession(string conversationId);

        /// <summary>
        /// Takes the security token from the session pool asynchronously.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        /// <returns>The security token.</returns>
        Task<Security> TakeSessionAsync(string conversationId);
    }
}