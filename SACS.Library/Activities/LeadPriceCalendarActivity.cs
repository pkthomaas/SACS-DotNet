using System.Collections.Generic;
using SACS.Library.Activities.InputData;
using SACS.Library.Rest;
using SACS.Library.Rest.Models.LeadPriceCalendar;
using SACS.Library.Workflow;

namespace SACS.Library.Activities
{
    /// <summary>
    /// The activity for LeadPriceCalendar REST call.
    /// </summary>
    public class LeadPriceCalendarActivity : IActivity
    {
        /// <summary>
        /// The shared context key for results
        /// </summary>
        public const string SharedContextKey = "LeadPriceCalendarActivity";

        /// <summary>
        /// The shared context key for response JSON
        /// </summary>
        public const string ResponseJsonSharedContextKey = "LeadPriceCalendarActivityResponseJson";

        /// <summary>
        /// The shared context key for request URL
        /// </summary>
        public const string RequestUrlSharedContextKey = "LeadPriceCalendarActivityRequestUrl";

        /// <summary>
        /// The endpoint
        /// </summary>
        private const string Endpoint = "v2/shop/flights/fares";

        /// <summary>
        /// The input data
        /// </summary>
        private readonly ILeadPriceCalendarData data;

        /// <summary>
        /// The REST client
        /// </summary>
        private readonly RestClient restClient;

        /// <summary>
        /// If set to <c>true</c>, then the activity is last in flow.
        /// </summary>
        private readonly bool lastInWorkflow;

        /// <summary>
        /// Initializes a new instance of the <see cref="LeadPriceCalendarActivity"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="restClient">The REST client.</param>
        /// <param name="lastInWorkflow">if set to <c>true</c>, then the activity is last in flow.</param>
        public LeadPriceCalendarActivity(ILeadPriceCalendarData data, RestClient restClient, bool lastInWorkflow = true)
        {
            this.data = data;
            this.restClient = restClient;
            this.lastInWorkflow = lastInWorkflow;
        }

        /// <summary>
        /// Runs the activity asynchronously.
        /// </summary>
        /// <param name="sharedContext">The shared context.</param>
        /// <returns>
        /// Next activity to be run or <c>null</c> if last in flow.
        /// </returns>
        public async System.Threading.Tasks.Task<IActivity> RunAsync(SharedContext sharedContext)
        {
            IDictionary<string, string> queryDictionary = new Dictionary<string, string>
            {
                { "origin", this.data.Origin },
                { "destination", this.data.Destination },
                { "lengthofstay", this.data.LengthOfStay.ToString() },
                { "pointofsalecountry", this.data.PointOfSaleCountry },
                { "departuredate", this.data.DepartureDate.ToString("yyyy-MM-dd") }
            };

            if (this.data.MinFare.HasValue)
            {
                queryDictionary.Add("minfare", this.data.MinFare.Value.ToString());
            }

            if (this.data.MaxFare.HasValue)
            {
                queryDictionary.Add("maxfare", this.data.MaxFare.Value.ToString());
            }

            var httpResponse = await this.restClient.GetAsync<LeadPriceCalendarRS>(Endpoint, queryDictionary);
            sharedContext.AddRestResult(SharedContextKey, httpResponse, true, ResponseJsonSharedContextKey);
            sharedContext.AddResult(RequestUrlSharedContextKey, httpResponse.RequestUri);
            return this.lastInWorkflow ? null : new InstaFlightsActivity(this.restClient);
        }
    }
}