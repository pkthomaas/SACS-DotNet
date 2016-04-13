using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using SACS.Library.SabreSoapApi;
using SACS.Library.Soap;
using SACS.Library.Workflow;

namespace SACS.Library.Activities
{
    /// <summary>
    /// The activity for TravelItineraryRead SOAP call.
    /// </summary>
    public class TravelItineraryReadActivity : IActivity
    {
        /// <summary>
        /// The shared context key for results
        /// </summary>
        public const string SharedContextKey = "TravelItineraryReadActivity";

        /// <summary>
        /// The shared context key for request presented as XML string
        /// </summary>
        public const string RequestXmlSharedContextKey = "TravelItineraryReadRequestXML";

        /// <summary>
        /// The shared context key for response presented as XML string
        /// </summary>
        public const string ResponseXmlSharedContextKey = "TravelItineraryReadResponseXML";

        /// <summary>
        /// The shared context key for the travel itinerary reference identifier
        /// </summary>
        public const string TravelItineraryRefContextKey = "PNR";

        /// <summary>
        /// The SOAP service factory
        /// </summary>
        private readonly SoapServiceFactory soapServiceFactory;

        /// <summary>
        /// The session pool
        /// </summary>
        private readonly ISessionPool sessionPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="TravelItineraryReadActivity"/> class.
        /// </summary>
        /// <param name="soapServiceFactory">The SOAP service factory.</param>
        /// <param name="sessionPool">The session pool.</param>
        public TravelItineraryReadActivity(SoapServiceFactory soapServiceFactory, ISessionPool sessionPool)
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
            TravelItineraryReadRQ request = new TravelItineraryReadRQ
            {
                Version = "3.6.0",
                MessagingDetails = new TravelItineraryReadRQMessagingDetails
                {
                    SubjectAreas = new[] { "PNR" }
                },
                UniqueID = new TravelItineraryReadRQUniqueID
                {
                    ID = sharedContext.GetResult<string>(TravelItineraryRefContextKey)
                }
            };
            var security = await this.sessionPool.TakeSessionAsync(sharedContext.ConversationId);
            var service = this.soapServiceFactory.CreateTravelItineraryReadService(sharedContext.ConversationId, security);
            sharedContext.AddSerializedResultXML(RequestXmlSharedContextKey, request);
            try
            {
                TravelItineraryReadRS response = service.TravelItineraryReadRQ(request);
                sharedContext.AddResult(SharedContextKey, response);
                sharedContext.AddSerializedResultXML(ResponseXmlSharedContextKey, response);
                return null;
            }
            catch (Exception ex)
            {
                sharedContext.AddResult(SharedContextKey, ex);
                sharedContext.IsFaulty = true;
                this.sessionPool.ReleaseSession(sharedContext.ConversationId);
                return null;
            }
        }
    }
}