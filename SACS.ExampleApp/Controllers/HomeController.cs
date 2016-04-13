using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SACS.ExampleApp.Factories;

namespace SACS.ExampleApp.Controllers
{
    /// <summary>
    /// The home controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Display the application's index page.
        /// </summary>
        /// <returns>The view.</returns>
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
