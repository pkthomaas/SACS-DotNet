using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using SACS.ExampleApp.Factories;
using SACS.ExampleApp.Models;
using SACS.Library.Activities;
using SACS.Library.Rest;
using SACS.Library.Soap;
using SACS.Library.Workflow;

namespace SACS.ExampleApp.Controllers
{
    /// <summary>
    /// The controller for REST API calls.
    /// </summary>
    public class RestController : Controller
    {
        /// <summary>
        /// Performs the BargainFinderMax REST request and returns results.
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <returns>The view.</returns>
        [HttpPost]
        [Route("bargain_finder_max/search")]
        public async Task<ActionResult> BargainFinderMax(BargainFinderMaxPostRQ requestModel)
        {
            IActivity activity = new BargainFinderMaxActivity(RestClientFactory.Create(), requestModel);
            Workflow workflow = new Workflow(activity);
            SharedContext sharedContext = await workflow.RunAsync();
            BargainFinderMaxVM model = ViewModelFactory.CreateBargainFinderMaxVM(sharedContext);
            return this.View(model);
        }

        /// <summary>
        /// Displays the BargainFinderMax form.
        /// </summary>
        /// <returns>The view.</returns>
        [HttpGet]
        [Route("bargain_finder_max")]
        public async Task<ActionResult> BargainFinderMaxForm()
        {
            ViewBag.Airports = await AirportCodesFactory.CreateAirportCodes();
            return this.View();
        }

        /// <summary>
        /// Performs the InstaFlights Search REST request and returns results.
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <returns>The view.</returns>
        [HttpPost]
        [Route("insta_flights_search/search")]
        public async Task<ActionResult> InstaFlightsSearch(InstaFlightsPostRQ requestModel)
        {
            RestClient restClient = RestClientFactory.Create();
            IActivity activity = new InstaFlightsActivity(restClient, requestModel);
            Workflow workflow = new Workflow(activity);
            SharedContext sharedContext = await workflow.RunAsync();
            InstaFlightsPostVM viewModel = ViewModelFactory.CreateInstaFlightsVM(sharedContext);
            return this.View(viewModel);
        }

        /// <summary>
        /// Displays the InstaFlights Search form.
        /// </summary>
        /// <returns>The view.</returns>
        [HttpGet]
        [Route("insta_flights_search")]
        public async Task<ActionResult> InstaFlightsSearchForm()
        {
            ViewBag.Airports = await AirportCodesFactory.CreateAirportCodes();
            return this.View();
        }

        /// <summary>
        /// Performs the LeadPriceCalendar REST request and returns results.
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <returns>The view.</returns>
        [HttpPost]
        [Route("lead_price_calendar/search")]
        public async Task<ActionResult> LeadPriceCalendar(LeadPriceCalendarPostRQ requestModel)
        {
            RestClient restClient = RestClientFactory.Create();
            IActivity activity = new LeadPriceCalendarActivity(requestModel, restClient);
            Workflow workflow = new Workflow(activity);
            SharedContext sharedContext = await workflow.RunAsync();
            LeadPriceCalendarPostVM viewModel = ViewModelFactory.CreateLeadPriceCalendarVM(sharedContext);
            return this.View(viewModel);
        }

        /// <summary>
        /// Display the LeadPriceCalendar form.
        /// </summary>
        /// <returns>The view.</returns>
        [HttpGet]
        [Route("lead_price_calendar")]
        public async Task<ActionResult> LeadPriceCalendarForm()
        {
            ViewBag.Airports = await AirportCodesFactory.CreateAirportCodes();
            return this.View();
        }
    }
}