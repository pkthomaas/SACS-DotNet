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
    /// The activity for SOAP BargainFinderMax call
    /// </summary>
    public class BargainFinderMaxSoapActivity : IActivity
    {
        /// <summary>
        /// The shared context key for input data.
        /// </summary>
        public const string InputDataSharedContextKey = "BargainFinderMaxSoapInputData";

        /// <summary>
        /// The shared context key for request serialized as XML
        /// </summary>
        public const string RequestXmlSharedContextKey = "BargainFinderMaxSoapRequestXml";

        /// <summary>
        /// The shared context key for response serialized as XML
        /// </summary>
        public const string ResponseXmlSharedContextKey = "BargainFinderMaxSoapResponseXml";

        /// <summary>
        /// The SOAP service factory
        /// </summary>
        private readonly SoapServiceFactory soapServiceFactory;

        /// <summary>
        /// The session pool
        /// </summary>
        private readonly ISessionPool sessionPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="BargainFinderMaxSoapActivity" /> class.
        /// </summary>
        /// <param name="soapServiceFactory">The SOAP service factory.</param>
        /// <param name="sessionPool">The session pool.</param>
        public BargainFinderMaxSoapActivity(SoapServiceFactory soapServiceFactory, ISessionPool sessionPool)
        {
            this.sessionPool = sessionPool;
            this.soapServiceFactory = soapServiceFactory;
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
            IBargainFinderMaxData data = sharedContext.GetResult<IBargainFinderMaxData>(InputDataSharedContextKey);
            if (data == null)
            {
                throw new ArgumentException("No data found under key: " + InputDataSharedContextKey);
            }

            var security = await this.sessionPool.TakeSessionAsync(sharedContext.ConversationId);
            var service = this.soapServiceFactory.CreateBargainFinderMaxService(sharedContext.ConversationId, security);
            var request = this.CreateRequest(data);
            sharedContext.AddSerializedResultXML(RequestXmlSharedContextKey, request);
            try
            {
                var result = service.BargainFinderMaxRQ(request);
                sharedContext.AddSerializedResultXML(ResponseXmlSharedContextKey, result);
                EnhancedAirBookData enhancedAirBookData = this.CreateEnhancedAirBookData(result);
                sharedContext.AddResult(EnhancedAirBookActivity.InputDataSharedContextKey, enhancedAirBookData);
                return new PassengerDetailsContactActivity(this.soapServiceFactory, this.sessionPool);
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
        /// Creates the input data for <see cref="EnhancedAirBookActivity"/>.
        /// </summary>
        /// <param name="result">The BargainFinderMax result.</param>
        /// <returns>The data.</returns>
        private EnhancedAirBookData CreateEnhancedAirBookData(OTA_AirLowFareSearchRS result)
        {
            OTA_AirLowFareSearchRSPricedItineraries itineraries = result.Items.OfType<OTA_AirLowFareSearchRSPricedItineraries>().First();
            EnhancedAirBookData enhancedAirBookData = new EnhancedAirBookData();
            enhancedAirBookData.FlightSegments = itineraries.PricedItinerary.First().AirItinerary.OriginDestinationOptions.First().FlightSegment
            .Select(flightSegment => new FlightSegmentData
            {
                DepartureDateTime = DateTime.ParseExact(flightSegment.DepartureDateTime, "s", CultureInfo.InvariantCulture),
                DestinationLocationCode = flightSegment.ArrivalAirport.LocationCode,
                FlightNumber = flightSegment.FlightNumber,
                MarketingAirlineCode = flightSegment.MarketingAirline.Code,
                OriginLocationCode = flightSegment.DepartureAirport.LocationCode,
                DesignationCode = flightSegment.ResBookDesigCode
            });
            return enhancedAirBookData;
        }

        /// <summary>
        /// Creates the request.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// The request.
        /// </returns>
        private OTA_AirLowFareSearchRQ CreateRequest(IBargainFinderMaxData data)
        {
            return new OTA_AirLowFareSearchRQ
            {
                Version = "1.9.2",
                ResponseType = "OTA",
                ResponseVersion = "1.9.2",
                OriginDestinationInformation = this.CreateOriginDestinationInfos(data),
                POS = this.CreatePOS(data),
                TPA_Extensions = this.CreateTPAExtensions(data),
                TravelerInfoSummary = this.CreateTravelInfoSummary(data),
                TravelPreferences = this.CreateTravelPreferences(data)
            };
        }

        /// <summary>
        /// Creates the travel preferences model.
        /// </summary>
        /// <param name="bargainFinderMaxData">The bargain finder maximum data.</param>
        /// <returns>The model.</returns>
        private AirSearchPrefsType CreateTravelPreferences(IBargainFinderMaxData bargainFinderMaxData)
        {
            return new AirSearchPrefsType
            {
                TPA_Extensions = new AirSearchPrefsTypeTPA_Extensions
                {
                    NumTrips = new NumTripsType
                    {
                        Number = 1
                    }
                }
            };
        }

        /// <summary>
        /// Creates the travel information summary.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The summary.</returns>
        private TravelerInfoSummaryType CreateTravelInfoSummary(IBargainFinderMaxData data)
        {
            return new TravelerInfoSummaryType
            {
                AirTravelerAvail = new TravelerInformationType[] 
                { 
                    new TravelerInformationType 
                    {
                        PassengerTypeQuantity = new PassengerTypeQuantityType[] 
                        {
                            new PassengerTypeQuantityType 
                            {
                                Code = data.PassengerTypeCode,
                                Quantity = data.NumberOfPassengers.ToString()
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Creates the TPA extensions model.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The model.</returns>
        private OTA_AirLowFareSearchRQTPA_Extensions CreateTPAExtensions(IBargainFinderMaxData data)
        {
            return new OTA_AirLowFareSearchRQTPA_Extensions
            {
                IntelliSellTransaction = new TransactionType
                {
                    RequestType = new TransactionTypeRequestType
                    {
                        Name = data.RequestType ?? string.Empty
                    }
                }
            };
        }

        /// <summary>
        /// Creates the POS model.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The model.</returns>
        private SourceType2[] CreatePOS(IBargainFinderMaxData data)
        {
            return new SourceType2[] 
            {
                new SourceType2 
                {
                    RequestorID = new UniqueID_Type
                    {
                        Type = data.RequestorType,
                        CompanyName = new CompanyNameType2 
                        { 
                            Code = data.RequestorCompanyCode
                        },
                        ID = data.RequestorID
                    },
                    PseudoCityCode = "7TZA"
                }
            };
        }

        /// <summary>
        /// Creates the origin destination information models.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The models.</returns>
        private OTA_AirLowFareSearchRQOriginDestinationInformation[] CreateOriginDestinationInfos(IBargainFinderMaxData data)
        {
            return new OTA_AirLowFareSearchRQOriginDestinationInformation[] 
            {
                new OTA_AirLowFareSearchRQOriginDestinationInformation 
                {
                    ItemElementName = ItemChoiceType1.DepartureDateTime,
                    Item = data.DepartureDate.ToString("s", CultureInfo.InvariantCulture),
                    OriginLocation = new OriginDestinationInformationTypeOriginLocation 
                    {
                        LocationCode = data.OriginAirportCode
                    },
                    DestinationLocation = new OriginDestinationInformationTypeDestinationLocation
                    {
                        LocationCode = data.DestinationAirportCode
                    },
                    RPH = data.RPH
                }
            };
        }
    }
}
