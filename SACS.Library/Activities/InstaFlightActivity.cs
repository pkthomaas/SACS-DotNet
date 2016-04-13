using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SACS.Library.Activities.InputData;
using SACS.Library.Rest;
using SACS.Library.Rest.Models.InstaFlight;
using SACS.Library.Rest.Models.LeadPriceCalendar;
using SACS.Library.Workflow;

namespace SACS.Library.Activities
{
    /// <summary>
    /// The activity for InstaFlights Search REST call.
    /// </summary>
    public class InstaFlightsActivity : IActivity
    {
        /// <summary>
        /// The shared context key for JSON response.
        /// </summary>
        public const string JsonSharedContextKey = "InstaFlightsJson";

        /// <summary>
        /// The shared context key for request URI.
        /// </summary>
        public const string RequestUriSharedContextKey = "InstaFlightsRequestUri";

        /// <summary>
        /// The shared context key for results.
        /// </summary>
        public const string SharedContextKey = "InstaFlightsSearchActivity";

        /// <summary>
        /// The endpoint.
        /// </summary>
        private const string Endpoint = "/v1/shop/flights";

        /// <summary>
        /// The input data
        /// </summary>
        private readonly IInstaFlightsData data;

        /// <summary>
        /// The REST client
        /// </summary>
        private readonly RestClient restClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstaFlightsActivity"/> class.
        /// </summary>
        /// <param name="restClient">The REST client.</param>
        /// <param name="data">The input data.</param>
        public InstaFlightsActivity(RestClient restClient, IInstaFlightsData data = null)
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
            LeadPriceCalendarRS leadPriceCalendarRS = sharedContext.GetResult(LeadPriceCalendarActivity.SharedContextKey) as LeadPriceCalendarRS;
            HttpResponse<InstaFlightRS> httpResponse;
            if (this.data != null)
            {
                Dictionary<string, string> queryDictionary = new Dictionary<string, string>
                {
                    { "origin", this.data.Origin },
                    { "destination", this.data.Destination },
                    { "departuredate", this.data.DepartureDate.ToString("yyyy-MM-dd") },
                    { "returndate", this.data.ReturnDate.ToString("yyyy-MM-dd") }
                };
                httpResponse = await this.restClient.GetAsync<InstaFlightRS>(Endpoint, queryDictionary);
            }
            else
            {
                string uri = leadPriceCalendarRS.FareInfo.SelectMany(fi => fi.Links).Where(l => l.Rel == "shop").Select(l => l.Href).First();
                httpResponse = await this.restClient.GetAsync<InstaFlightRS>(uri);
            }

            sharedContext.AddRestResult(SharedContextKey, httpResponse, true, JsonSharedContextKey);
            sharedContext.AddResult(RequestUriSharedContextKey, httpResponse.RequestUri);
            return null;
        }
    }
}