using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACS.ExampleApp.Models
{    
    /// <summary>
    /// The view model for displaying airport dropdowns.
    /// </summary>
    public class AirportModel
    {
        /// <summary>
        /// Gets or sets the airport code.
        /// </summary>
        /// <value>
        /// The airport code.
        /// </value>
        public string AirportCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the airport.
        /// </summary>
        /// <value>
        /// The name of the airport.
        /// </value>
        public string AirportName { get; set; }
    }
}