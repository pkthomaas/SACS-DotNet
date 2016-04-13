using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACS.ExampleApp.Models
{
    /// <summary>
    /// The view model for aiport dropdowns
    /// </summary>
    public class AirportDropdownVM
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AirportDropdownVM"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="airportCode">The airport code.</param>
        public AirportDropdownVM(string name, string airportCode)
        {
            this.Name = name;
            this.AirportCode = airportCode;
        }

        /// <summary>
        /// Gets or sets the airport code.
        /// </summary>
        /// <value>
        /// The airport code.
        /// </value>
        public string AirportCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the dropdown.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}