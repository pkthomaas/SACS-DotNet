using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SACS.ExampleApp.Factories;
using SACS.ExampleApp.Models;
using SACS.Library.Activities;
using SACS.Library.Soap;
using SACS.Library.Workflow;

namespace SACS.ExampleApp.Controllers
{
    /// <summary>
    /// The controller for SOAP API calls.
    /// </summary>
    public class SoapController : Controller
    {
        /// <summary>
        /// The session pool
        /// </summary>
        private static readonly ISessionPool SessionPool = SessionPoolFactory.CreateSimple();

        /// <summary>
        /// Performs the BargainFinderMax, PassengerDetails, EnhancedAirBook and TravelItineraryRead SOAP requests in sequence and returns results.
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <returns>The view.</returns>
        [HttpPost]
        [Route("soap_workflow/search")]
        public async Task<ActionResult> SoapWorkflow(SoapWorkflowPostRQ requestModel)
        {
            IActivity activity = new InitialSoapActivity(new SoapServiceFactory(ConfigFactory.CreateForSoap()), SessionPool, requestModel);
            Workflow workflow = new Workflow(activity);
            SharedContext sharedContext = await workflow.RunAsync();
            SoapWorkflowVM model = ViewModelFactory.CreateSoapWorkflowVM(sharedContext);
            return this.View(model);
        }

        /// <summary>
        /// Displays the SOAP workflow form.
        /// </summary>
        /// <returns>The view.</returns>
        [HttpGet]
        [Route("soap_workflow")]
        public async Task<ActionResult> SoapWorkflowForm()
        {
            ViewBag.Airports = await AirportCodesFactory.CreateAirportCodes();
            List<SelectListItem> miscSegmentTypes = ViewModelFactory.CreateMiscSegmentTypesList();
            return this.View(miscSegmentTypes);
        }
    }
}