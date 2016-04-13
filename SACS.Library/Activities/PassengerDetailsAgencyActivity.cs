using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.Library.Activities.InputData;
using SACS.Library.SabreSoapApi;
using SACS.Library.Soap;
using SACS.Library.Workflow;

namespace SACS.Library.Activities
{
    /// <summary>
    /// The activity for PassengerDetails SOAP call that sets agency address and other information needed for completing the PNR.
    /// </summary>
    public class PassengerDetailsAgencyActivity : IActivity
    {
        /// <summary>
        /// The shared context key for results
        /// </summary>
        public const string SharedContextKey = "PassengerDetailsAgencyResult";

        /// <summary>
        /// The shared context key for input data.
        /// </summary>
        public const string InputDataSharedContextKey = "PassengerDetailsAgencyInputData";

        /// <summary>
        /// The shared context key for request serialized as XML
        /// </summary>
        public const string RequestXmlSharedContextKey = "PassengerDetailsAgencyRequestXml";

        /// <summary>
        /// The shared context key for response serialized as XML
        /// </summary>
        public const string ResponseXmlSharedContextKey = "PassengerDetailsAgencyResponseXml";

        /// <summary>
        /// The SOAP service factory
        /// </summary>
        private readonly SoapServiceFactory soapServiceFactory;

        /// <summary>
        /// The session pool
        /// </summary>
        private readonly ISessionPool sessionPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="PassengerDetailsAgencyActivity"/> class.
        /// </summary>
        /// <param name="soapServiceFactory">The SOAP service factory.</param>
        /// <param name="sessionPool">The session pool.</param>
        public PassengerDetailsAgencyActivity(SoapServiceFactory soapServiceFactory, ISessionPool sessionPool)
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
            var security = await this.sessionPool.TakeSessionAsync(sharedContext.ConversationId);
            var service = this.soapServiceFactory.CreatePassengerDetailsService(sharedContext.ConversationId, security);
            IPassengerDetailsActivityData data = sharedContext.GetResult<IPassengerDetailsActivityData>(InputDataSharedContextKey);
            PassengerDetailsRQ request = this.CreateRequest(data);
            sharedContext.AddSerializedResultXML(RequestXmlSharedContextKey, request);
            try
            {
                PassengerDetailsRS response = service.PassengerDetailsRQ(request);
                sharedContext.AddResult(SharedContextKey, response);
                sharedContext.AddSerializedResultXML(ResponseXmlSharedContextKey, response);
                sharedContext.AddResult(TravelItineraryReadActivity.TravelItineraryRefContextKey, response.ItineraryRef.ID);
                return new TravelItineraryReadActivity(this.soapServiceFactory, this.sessionPool);
            }
            catch (Exception ex)
            {
                sharedContext.AddResult(CommonConstants.ExceptionSharedContextKey, ex);
                sharedContext.IsFaulty = true;
                this.sessionPool.ReleaseSession(sharedContext.ConversationId);
                return null;
            }
        }

        /// <summary>
        /// Creates the request.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// The PassengerDetails request.
        /// </returns>
        private PassengerDetailsRQ CreateRequest(IPassengerDetailsActivityData data)
        {
            return new PassengerDetailsRQ
            {
                IgnoreOnError2 = true,
                version = "3.2.0",
                TravelItineraryAddInfoRQ = this.CreateTravelItineraryAddInfo(data),
                PostProcessing = new PassengerDetailsRQPostProcessing
                {
                    EndTransactionRQ = new PassengerDetailsRQPostProcessingEndTransactionRQ
                    {
                        EndTransaction = new PassengerDetailsRQPostProcessingEndTransactionRQEndTransaction { Ind = "true" },
                        Source = new PassengerDetailsRQPostProcessingEndTransactionRQSource
                        {
                            ReceivedFrom = string.Format("{0} {1}", data.GivenName, data.Surname)
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Creates the TravelItineraryAddInfo request model.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// The model.
        /// </returns>
        private PassengerDetailsRQTravelItineraryAddInfoRQ CreateTravelItineraryAddInfo(IPassengerDetailsActivityData data)
        {
            return new PassengerDetailsRQTravelItineraryAddInfoRQ
            {
                AgencyInfo = this.CreateAgencyInfo(data)
            };
        }

        /// <summary>
        /// Creates the model containing agency information.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// The model.
        /// </returns>
        private PassengerDetailsRQTravelItineraryAddInfoRQAgencyInfo CreateAgencyInfo(IPassengerDetailsActivityData data)
        {
            return new PassengerDetailsRQTravelItineraryAddInfoRQAgencyInfo
            {
                Address = new PassengerDetailsRQTravelItineraryAddInfoRQAgencyInfoAddress
                {
                    AddressLine = data.AgencyAddressLine,
                    CityName = data.AgencyCityName,
                    CountryCode = data.AgencyCountryCode,
                    PostalCode = data.AgencyPostalCode,
                    StreetNmbr = data.AgencyStreetNumber,
                    StateCountyProv = new PassengerDetailsRQTravelItineraryAddInfoRQAgencyInfoAddressStateCountyProv
                    {
                        StateCode = data.AgencyStateCode
                    }
                },
                Ticketing = new PassengerDetailsRQTravelItineraryAddInfoRQAgencyInfoTicketing
                {
                    TicketType = data.AgencyTicketType
                }
            };
        }
    }
}
