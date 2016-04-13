using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SACS.Library.Activities.InputData;
using SACS.Library.Rest;
using SACS.Library.Rest.Models.BargainFinderMax;
using SACS.Library.Rest.Models.Common;
using SACS.Library.Serialization;
using SACS.Library.Workflow;

namespace SACS.Library.Activities
{
    /// <summary>
    /// The activity for BargainFinderMax REST call.
    /// </summary>
    public class BargainFinderMaxActivity : IActivity
    {
        /// <summary>
        /// The shared context key for results.
        /// </summary>
        public const string SharedContextKey = "BargainFinderMaxActivity";

        /// <summary>
        /// The shared context key for response JSON.
        /// </summary>
        public const string ResponseJsonSharedContextKey = "BargainFinderMaxActivityResponseJson";

        /// <summary>
        /// The shared context key for request JSON.
        /// </summary>
        public const string RequestJsonSharedContextKey = "BargainFinderMaxActivityRequestJson";

        /// <summary>
        /// The shared context key for request URL.
        /// </summary>
        public const string RequestUrlSharedContextKey = "BargainFinderMaxActivityRequestUrl";

        /// <summary>
        /// The endpoint
        /// </summary>
        private const string Endpoint = "/v1.9.2/shop/flights?mode=live";

        /// <summary>
        /// The REST client
        /// </summary>
        private readonly RestClient restClient;

        /// <summary>
        /// The input data
        /// </summary>
        private readonly IBargainFinderMaxData data;

        /// <summary>
        /// Initializes a new instance of the <see cref="BargainFinderMaxActivity"/> class.
        /// </summary>
        /// <param name="restClient">The REST client.</param>
        /// <param name="data">The input data.</param>
        public BargainFinderMaxActivity(RestClient restClient, IBargainFinderMaxData data)
        {
            this.restClient = restClient;
            this.data = data;
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
            BargainFinderMaxRQ request = this.CreateRequest(this.data);
            sharedContext.AddSerializedResultJson(RequestJsonSharedContextKey, request);
            var httpResponse = await this.restClient.PostAsync<BargainFinderMaxRQ, BargainFinderMaxRS>(Endpoint, request);
            sharedContext.AddRestResult<BargainFinderMaxRS>(SharedContextKey, httpResponse, false, ResponseJsonSharedContextKey);
            sharedContext.AddResult(RequestUrlSharedContextKey, httpResponse.RequestUri);
            return null;
        }

        /// <summary>
        /// Creates the request.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The BargainFinderMax request model.</returns>
        private BargainFinderMaxRQ CreateRequest(IBargainFinderMaxData data)
        {
            return new BargainFinderMaxRQ
            {
                OTAAirLowFareSearchRQ = new OTAAirLowFareSearchRQ
                {
                    OriginDestinationInformation = this.CreateOriginDestinationInfos(data),
                    POS = this.CreatePos(data),
                    TPAExtensions = this.CreateTPAExtensions(data),
                    TravelerInfoSummary = this.CreateTravelInfoSummary(data),
                    TravelPreferences = this.CreateTravelPreferences()
                }
            };
        }

        /// <summary>
        /// Creates the travel preferences.
        /// </summary>
        /// <returns>The travel preferences.</returns>
        private TravelPreferences CreateTravelPreferences()
        {
            return new TravelPreferences()
            {
                TPAExtensions = new TravelPreferencesTPAExtensions
                {
                    NumTrips = new NumTrips
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
        /// <returns>The travel information summary.</returns>
        private TravelerInfoSummary CreateTravelInfoSummary(IBargainFinderMaxData data)
        {
            return new TravelerInfoSummary
            {
                AirTravelerAvail = new List<AirTravelerAvail>
                {
                    new AirTravelerAvail
                    {
                        PassengerTypeQuantity = new List<PassengerTypeQuantity>
                        {
                            new PassengerTypeQuantity
                            {
                                Code = data.PassengerTypeCode,
                                Quantity = data.NumberOfPassengers
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
        /// <returns>The TPA extensions model.</returns>
        private BargainFinderMaxTPAExtensions CreateTPAExtensions(IBargainFinderMaxData data)
        {
            return new BargainFinderMaxTPAExtensions
            {
                IntelliSellTransaction = new IntelliSellTransaction
                {
                    RequestType = new RequestType
                    {
                        Name = data.RequestType ?? string.Empty
                    }
                }
            };
        }

        /// <summary>
        /// Creates the POS model..
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The POS model.</returns>
        private POS CreatePos(IBargainFinderMaxData data)
        {
            return new POS
            {
                Source = new List<Source>
                {
                    new Source
                    {
                        RequestorID = new RequestorID
                        {
                            ID = data.RequestorID,
                            Type = data.RequestorType,
                            CompanyName = new CompanyCode
                            {
                                Code = data.RequestorCompanyCode
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Creates the models containing origin and destination location.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The models containing origin and destination location.</returns>
        private List<OriginDestinationInformation> CreateOriginDestinationInfos(IBargainFinderMaxData data)
        {
            return new List<OriginDestinationInformation>
            {
                new OriginDestinationInformation
                {
                    DepartureDateTime = data.DepartureDate,
                    OriginLocation = new Location
                    {
                        LocationCode = data.OriginAirportCode
                    },
                    DestinationLocation = new Location
                    {
                        LocationCode = data.DestinationAirportCode
                    },
                    RPH = data.RPH
                }
            };
        }
    }
}