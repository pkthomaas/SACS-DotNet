using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.Library.SabreSoapApi;
using SACS.Library.Soap;
using SACS.Library.Workflow;

namespace SACS.Library.Activities
{
    /// <summary>
    /// The activity for IgnoreTransaction SOAP call.
    /// </summary>
    public class IgnoreTransactionActivity : IActivity
    {
        /// <summary>
        /// The shared context key for request serialized as XML
        /// </summary>
        public const string RequestXmlSharedContextKey = "IgnoreTransactionRequestXml";

        /// <summary>
        /// The shared context key for response serialized as XML
        /// </summary>
        public const string ResponseXmlSharedContextKey = "IgnoreTransactionResponseXml";

        /// <summary>
        /// The SOAP service factory
        /// </summary>
        private readonly SoapServiceFactory soapServiceFactory;

        /// <summary>
        /// The session pool
        /// </summary>
        private readonly ISessionPool sessionPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="IgnoreTransactionActivity" /> class.
        /// </summary>
        /// <param name="soapServiceFactory">The SOAP service factory.</param>
        /// <param name="sessionPool">The session pool.</param>
        public IgnoreTransactionActivity(SoapServiceFactory soapServiceFactory, ISessionPool sessionPool)
        {
            this.soapServiceFactory = soapServiceFactory;
            this.sessionPool = sessionPool;
        }

        /// <summary>
        /// Runs the activity asynchronously.
        /// </summary>
        /// <param name="sharedContext">The shared context.</param>
        /// <returns>
        /// Next activity to be run or <c>null</c> if last in flow.
        /// </returns>
        public async Task<IActivity> RunAsync(SharedContext sharedContext)
        {
            IgnoreTransactionRQ request = new IgnoreTransactionRQ { Version = "2.0.0" };
            sharedContext.AddSerializedResultXML(RequestXmlSharedContextKey, request);
            Security security = await this.sessionPool.TakeSessionAsync(sharedContext.ConversationId);
            IgnoreTransactionService service = this.soapServiceFactory.CreateIgnoreTransactionService(sharedContext.ConversationId, security);
            try
            {
                IgnoreTransactionRS response = service.IgnoreTransactionRQ(request);
                sharedContext.AddSerializedResultXML(ResponseXmlSharedContextKey, response);
            }
            catch (Exception ex)
            {
                sharedContext.AddResult(CommonConstants.ExceptionSharedContextKey, ex);
            }

            sharedContext.IsFaulty = true;
            this.sessionPool.ReleaseSession(sharedContext.ConversationId);
            return null;
        }
    }
}
