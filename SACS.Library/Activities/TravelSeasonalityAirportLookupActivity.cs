using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.Library.Rest;
using SACS.Library.Rest.Models.TravelSeasonalityAirportsLookup;
using SACS.Library.Workflow;

namespace SACS.Library.Activities
{
    /// <summary>
    /// The activity for TravelSeasonalityAirportLookup REST call.
    /// </summary>
    public class TravelSeasonalityAirportLookupActivity : IActivity
    {
        /// <summary>
        /// The shared context key for storing results
        /// </summary>
        public const string SharedContextKey = "TravelSeasonalityAirportLookup";
        
        /// <summary>
        /// The endpoint
        /// </summary>
        private static readonly string Endpoint = "/v1/lists/supported/historical/seasonality/airports";

        /// <summary>
        /// The REST client
        /// </summary>
        private readonly RestClient restClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TravelSeasonalityAirportLookupActivity"/> class.
        /// </summary>
        /// <param name="restClient">The REST client.</param>
        public TravelSeasonalityAirportLookupActivity(RestClient restClient)
        {
            this.restClient = restClient;
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
            var httpResult = await this.restClient.GetAsync<TravelSeasonalityAirportsLookupRS>(Endpoint);
            List<DestinationLocation> destinations = new List<DestinationLocation>();
            if (httpResult.IsSuccess)
            {
                destinations.AddRange(httpResult.Value.DestinationLocations.Select(d => d.DestinationLocation));
            }

            sharedContext.AddResult(SharedContextKey, destinations);
            return null;
        }
    }
}
