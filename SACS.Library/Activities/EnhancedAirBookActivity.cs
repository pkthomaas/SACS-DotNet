using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// The activity for EnhancedAirBook SOAP call.
    /// </summary>
    public class EnhancedAirBookActivity : IActivity
    {
        /// <summary>
        /// The shared context key
        /// </summary>
        public const string SharedContextKey = "EnhancedAirBookActivity";

        /// <summary>
        /// The input data shared context key
        /// </summary>
        public const string InputDataSharedContextKey = "EnhancedAirBookInputData";

        /// <summary>
        /// The shared context key for request serialized as XML
        /// </summary>
        public const string RequestXmlSharedContextKey = "EnhancedAirBookRequestXml";

        /// <summary>
        /// The shared context key for response serialized as XML
        /// </summary>
        public const string ResponseXmlSharedContextKey = "EnhancedAirBookResponseXml";

        /// <summary>
        /// The SOAP service factory
        /// </summary>
        private readonly SoapServiceFactory soapServiceFactory;

        /// <summary>
        /// The session pool
        /// </summary>
        private readonly ISessionPool sessionPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnhancedAirBookActivity"/> class.
        /// </summary>
        /// <param name="soapServiceFactory">The SOAP service factory.</param>
        /// <param name="sessionPool">The session pool.</param>
        public EnhancedAirBookActivity(SoapServiceFactory soapServiceFactory, ISessionPool sessionPool)
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
        /// <exception cref="ArgumentException">No data found under key:  + InputDataSharedContextKey</exception>
        public async Task<IActivity> RunAsync(SharedContext sharedContext)
        {
            var security = await this.sessionPool.TakeSessionAsync(sharedContext.ConversationId);
            var service = this.soapServiceFactory.CreateEnhancedAirBookService(sharedContext.ConversationId, security);
            IEnhancedAirBookData data = sharedContext.GetResult<IEnhancedAirBookData>(InputDataSharedContextKey);
            if (data == null) 
            {
                throw new ArgumentException("No data found under key: " + InputDataSharedContextKey);
            }

            EnhancedAirBookRQ request = this.CreateRequest(data);
            sharedContext.AddSerializedResultXML(RequestXmlSharedContextKey, request);
            try
            {
                var result = service.EnhancedAirBookRQ(request);
                sharedContext.AddSerializedResultXML(ResponseXmlSharedContextKey, result);
                return new PassengerDetailsAgencyActivity(this.soapServiceFactory, this.sessionPool);
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
        /// <returns>The EnhancedAirBook request.</returns>
        private EnhancedAirBookRQ CreateRequest(IEnhancedAirBookData data)
        {
            return new EnhancedAirBookRQ
            {
                version = "3.2.0",
                IgnoreOnError2 = true,
                OTA_AirBookRQ = this.CreateAirBookRequest(data)
            };
        }

        /// <summary>
        /// Creates the air book request.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The request</returns>
        private EnhancedAirBookRQOTA_AirBookRQ CreateAirBookRequest(IEnhancedAirBookData data)
        {
            return new EnhancedAirBookRQOTA_AirBookRQ
            {
                OriginDestinationInformation = data.FlightSegments.Select(flightSegment =>
                new EnhancedAirBookRQOTA_AirBookRQFlightSegment
                {
                    DepartureDateTime = flightSegment.DepartureDateTime.ToString("s", CultureInfo.InvariantCulture),
                    FlightNumber = flightSegment.FlightNumber,
                    NumberInParty = "1",
                    ResBookDesigCode = flightSegment.DesignationCode,
                    Status = "NN",
                    DestinationLocation = new EnhancedAirBookRQOTA_AirBookRQFlightSegmentDestinationLocation { LocationCode = flightSegment.DestinationLocationCode },
                    OriginLocation = new EnhancedAirBookRQOTA_AirBookRQFlightSegmentOriginLocation { LocationCode = flightSegment.OriginLocationCode },
                    MarketingAirline = new EnhancedAirBookRQOTA_AirBookRQFlightSegmentMarketingAirline
                    {
                        Code = flightSegment.MarketingAirlineCode,
                        FlightNumber = flightSegment.FlightNumber
                    }
                }).ToArray()
            };
        }
    }
}
