using System.Linq;
using System.Threading.Tasks;
using SACS.Library.SabreSoapApi;

namespace SACS.Library.Soap
{
    /// <summary>
    /// The SOAP authentication service.
    /// </summary>
    public class SoapAuth
    {
        /// <summary>
        /// The SOAP service factory
        /// </summary>
        private readonly SoapServiceFactory soapServiceFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoapAuth"/> class.
        /// </summary>
        /// <param name="soapServiceFactory">The SOAP service factory.</param>
        public SoapAuth(SoapServiceFactory soapServiceFactory)
        {
            this.soapServiceFactory = soapServiceFactory;
        }

        /// <summary>
        /// Creates a new session asynchronously.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        /// <returns>The SOAP result containing the SessionCreate response.</returns>
        public async Task<SoapResult<SessionCreateRS>> CreateSessionAsync(string conversationId)
        {
            SessionCreateRQ request = this.soapServiceFactory.CreateSessionCreateRequest();
            SessionCreateRQService service = this.soapServiceFactory.CreateSessionCreateService(conversationId);

            // This is an asynchronous SOAP call.
            // We need to convert event-based asynchrous call to task-based one.
            // The event is triggered when the action is completed.
            // Then it sets the result and ends the task.
            var source = new TaskCompletionSource<SoapResult<SessionCreateRS>>();
            service.SessionCreateRQCompleted += (s, e) =>
            {
                if (SoapHelper.HandleErrors(e, source))
                {
                    source.TrySetResult(SoapResult<SessionCreateRS>.Success(e.Result, service.SecurityValue));
                }
            };
            service.SessionCreateRQAsync(request);

            // Return the asynchronous task.
            return await source.Task;
        }

        /// <summary>
        /// Asynchronously tries to perform the refresh request.
        /// </summary>
        /// <param name="security">The security.</param>
        /// <param name="conversationId">The conversation identifier.</param>
        /// <returns>The SOAP result of the Ping response.</returns>
        public async Task<SoapResult<OTA_PingRS>> TryRefreshAsync(Security security, string conversationId)
        {
            OTA_PingRQ request = this.soapServiceFactory.CreatePingRequest();
            SWSService service = this.soapServiceFactory.CreatePingService(conversationId, security);
            var source = new TaskCompletionSource<SoapResult<OTA_PingRS>>();
            service.OTA_PingRQCompleted += (s, e) =>
            {
                if (SoapHelper.HandleErrors(e, source))
                {
                    if (e.Result.Items.Any())
                    {
                        ErrorsType error = e.Result.Items[0] as ErrorsType;
                        if (error != null && error.Error != null && error.Error.Length > 0)
                        {
                            source.TrySetResult(SoapResult<OTA_PingRS>.Error(error.Error.First()));
                            return;
                        }
                    }

                    source.TrySetResult(SoapResult<OTA_PingRS>.Success(e.Result));
                }
            };
            service.OTA_PingRQAsync(request);
            return await source.Task;
        }
    }
}