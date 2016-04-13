using System;
using System.Linq;
using System.Threading.Tasks;
using SACS.Library.Activities.InputData;
using SACS.Library.SabreSoapApi;
using SACS.Library.Soap;
using SACS.Library.Workflow;

namespace SACS.Library.Activities
{
    /// <summary>
    /// The activity for PassengerDetails SOAP call that sets passenger name and contact information.
    /// </summary>
    public class PassengerDetailsContactActivity : IActivity
    {
        /// <summary>
        /// The shared context key for results
        /// </summary>
        public const string SharedContextKey = "PassengerDetailsContactResult";

        /// <summary>
        /// The shared context key for input data.
        /// </summary>
        public const string InputDataSharedContextKey = "PassengerDetailsContactInputData";

        /// <summary>
        /// The shared context key for request serialized as XML
        /// </summary>
        public const string RequestXmlSharedContextKey = "PassengerDetailsContactRequestXml";

        /// <summary>
        /// The shared context key for response serialized as XML
        /// </summary>
        public const string ResponseXmlSharedContextKey = "PassengerDetailsContactResponseXml";

        /// <summary>
        /// The SOAP service factory
        /// </summary>
        private readonly SoapServiceFactory soapServiceFactory;

        /// <summary>
        /// The session pool
        /// </summary>
        private readonly ISessionPool sessionPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="PassengerDetailsContactActivity" /> class.
        /// </summary>
        /// <param name="soapServiceFactory">The SOAP service factory.</param>
        /// <param name="sessionPool">The session pool.</param>
        public PassengerDetailsContactActivity(SoapServiceFactory soapServiceFactory, ISessionPool sessionPool)
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
                return new EnhancedAirBookActivity(this.soapServiceFactory, this.sessionPool);
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
                TravelItineraryAddInfoRQ = this.CreateTravelItineraryAddInfo(data)
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
                CustomerInfo = new PassengerDetailsRQTravelItineraryAddInfoRQCustomerInfo
                {
                    ContactNumbers = this.CreateContactNumbers(data),
                    Email = new PassengerDetailsRQTravelItineraryAddInfoRQCustomerInfoEmail[]
                    {
                        new PassengerDetailsRQTravelItineraryAddInfoRQCustomerInfoEmail
                        {
                            NameNumber = "1.1",
                            Address = data.PassengerEmail,
                        }
                    },
                    PersonName = new PassengerDetailsRQTravelItineraryAddInfoRQCustomerInfoPersonName[]
                    {
                        new PassengerDetailsRQTravelItineraryAddInfoRQCustomerInfoPersonName
                        {
                            NameNumber = "1.1",
                            GivenName = data.GivenName,
                            Surname = data.Surname
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Creates the model containing contact numbers.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// The model.
        /// </returns>
        private PassengerDetailsRQTravelItineraryAddInfoRQCustomerInfoContactNumber[] CreateContactNumbers(IPassengerDetailsActivityData data)
        {
            return new PassengerDetailsRQTravelItineraryAddInfoRQCustomerInfoContactNumber[]
            {
                new PassengerDetailsRQTravelItineraryAddInfoRQCustomerInfoContactNumber
                {
                    LocationCode = data.Phone1LocationCode,
                    NameNumber = "1.1",
                    Phone = data.Phone1,
                    PhoneUseType = data.Phone1UseType
                },
                new PassengerDetailsRQTravelItineraryAddInfoRQCustomerInfoContactNumber
                {
                    LocationCode = data.Phone2LocationCode,
                    NameNumber = "1.1",
                    Phone = data.Phone2,
                    PhoneUseType = data.Phone2UseType
                },
            };
        }
    }
}