using System;

namespace SACS.ExampleApp.Models
{
    public class FareInfoVM
    {
        /// <summary>
        /// Gets the departure date time.
        /// </summary>
        /// <value>
        /// The departure date time.
        /// </value>
        public DateTime DepartureDateTime { get; set; }

        /// <summary>
        /// Gets the return date time.
        /// </summary>
        /// <value>
        /// The return date time.
        /// </value>
        public DateTime ReturnDateTime { get; set; }

        /// <summary>
        /// Gets the currency code.
        /// </summary>
        /// <value>
        /// The currency code.
        /// </value>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets the lowest fare.
        /// </summary>
        /// <value>
        /// The lowest fare.
        /// </value>
        public decimal LowestFare { get; set; }

        /// <summary>
        /// Gets the airline codes.
        /// </summary>
        /// <value>
        /// The airline codes.
        /// </value>
        public string AirlineCodes { get; set; }
    }
}