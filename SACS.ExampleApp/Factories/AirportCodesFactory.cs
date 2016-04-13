using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using SACS.ExampleApp.Models;
using SACS.Library.Activities;
using SACS.Library.Rest.Models.TravelSeasonalityAirportsLookup;
using SACS.Library.Workflow;

namespace SACS.ExampleApp.Factories
{
    /// <summary>
    /// The static factory for airport codes.
    /// </summary>
    public static class AirportCodesFactory
    {
        /// <summary>
        /// The airport codes
        /// </summary>
        private static IEnumerable<AirportModel> airportCodes;

        /// <summary>
        /// Creates the airport codes. If already created, will return existing values.
        /// Otherwise, will call TopDestinations API, parse and save the results.
        /// </summary>
        /// <returns>The airport codes.</returns>
        public static async Task<IEnumerable<AirportModel>> CreateAirportCodes()
        {
            if (airportCodes == null)
            {
                IActivity activity = new TravelSeasonalityAirportLookupActivity(RestClientFactory.Create());
                Workflow workflow = new Workflow(activity);
                SharedContext sharedContext = await workflow.RunAsync();

                if (sharedContext.IsFaulty)
                {
                    airportCodes = new List<AirportModel>();
                }
                else
                {
                    airportCodes = CreateAirportCodes(sharedContext);
                }
            }

            return airportCodes;
        }

        /// <summary>
        /// Creates the airport codes.
        /// </summary>
        /// <param name="sharedContext">The shared context.</param>
        /// <returns>The airport codes.</returns>
        public static IEnumerable<AirportModel> CreateAirportCodes(SharedContext sharedContext)
        {
            List<DestinationLocation> destinations = sharedContext.GetResult<List<DestinationLocation>>(TravelSeasonalityAirportLookupActivity.SharedContextKey);
            return destinations.Select(d => new AirportModel
            {
                AirportName = d.AirportName,
                AirportCode = d.AirportCode,
            }).ToList();
        }
    }
}