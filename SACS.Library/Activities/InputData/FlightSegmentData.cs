using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Library.Activities.InputData
{
    /// <summary>
    /// The data about flight segments
    /// </summary>
    public class FlightSegmentData : IFlightSegmentData
    {
        /// <summary>
        /// Gets or sets the departure date time.
        /// </summary>
        /// <value>
        /// The departure date time.
        /// </value>
        public DateTime DepartureDateTime { get; set; }

        /// <summary>
        /// Gets or sets the flight number.
        /// </summary>
        /// <value>
        /// The flight number.
        /// </value>
        public string FlightNumber { get; set; }

        /// <summary>
        /// Gets or sets the marketing airline code.
        /// </summary>
        /// <value>
        /// The marketing airline code.
        /// </value>
        public string MarketingAirlineCode { get; set; }

        /// <summary>
        /// Gets or sets the destination location (airport) code.
        /// </summary>
        /// <value>
        /// The destination location code.
        /// </value>
        public string DestinationLocationCode { get; set; }

        /// <summary>
        /// Gets or sets the origin location (airport) code.
        /// </summary>
        /// <value>
        /// The origin location code.
        /// </value>
        public string OriginLocationCode { get; set; }

        /// <summary>
        /// Gets or sets the designation code (<c>ResBookDesigCode</c>).
        /// </summary>
        /// <value>
        /// The designation code.
        /// </value>
        public string DesignationCode { get; set; }
    }
}
