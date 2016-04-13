using System;
using SACS.Library.Activities.InputData;

namespace SACS.ExampleApp.Models
{
    /// <summary>
    /// View model for the LeadPriceCalendar action.
    /// </summary>
    public class LeadPriceCalendarPostRQ : ILeadPriceCalendarData
    {
        /// <summary>
        /// Gets or sets the origin airport code.
        /// </summary>
        /// <value>
        /// The origin.
        /// </value>
        public string Origin { get; set; }

        /// <summary>
        /// Gets or sets the destination airport code.
        /// </summary>
        /// <value>
        /// The destination.
        /// </value>
        public string Destination { get; set; }

        /// <summary>
        /// Gets or sets the point of sales country code.
        /// </summary>
        /// <value>
        /// The point of sales country.
        /// </value>
        public string PointOfSaleCountry { get; set; }

        /// <summary>
        /// Gets or sets the length of stay.
        /// </summary>
        /// <value>
        /// The length of stay.
        /// </value>
        public int LengthOfStay { get; set; }

        /// <summary>
        /// Gets or sets the minimum fare.
        /// </summary>
        /// <value>
        /// The minimum fare.
        /// </value>
        public int? MinFare { get; set; }

        /// <summary>
        /// Gets or sets the maximum fare.
        /// </summary>
        /// <value>
        /// The maximum fare.
        /// </value>
        public int? MaxFare { get; set; }

        /// <summary>
        /// Gets or sets the departure date.
        /// </summary>
        /// <value>
        /// The departure date.
        /// </value>
        public DateTime DepartureDate { get; set; }
    }
}