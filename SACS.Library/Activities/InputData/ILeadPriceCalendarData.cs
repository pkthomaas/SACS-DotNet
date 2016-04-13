using System;

namespace SACS.Library.Activities.InputData
{
    /// <summary>
    /// The data for <see cref="LeadPriceCalendarActivity"/>
    /// </summary>
    public interface ILeadPriceCalendarData
    {
        /// <summary>
        /// Gets or sets the destination.
        /// </summary>
        /// <value>
        /// The destination.
        /// </value>
        string Destination { get; set; }

        /// <summary>
        /// Gets or sets the length of stay.
        /// </summary>
        /// <value>
        /// The length of stay.
        /// </value>
        int LengthOfStay { get; set; }

        /// <summary>
        /// Gets or sets the origin.
        /// </summary>
        /// <value>
        /// The origin.
        /// </value>
        string Origin { get; set; }

        /// <summary>
        /// Gets or sets the point of sale country.
        /// </summary>
        /// <value>
        /// The point of sale country.
        /// </value>
        string PointOfSaleCountry { get; set; }

        /// <summary>
        /// Gets or sets the minimum fare.
        /// </summary>
        /// <value>
        /// The minimum fare.
        /// </value>
        int? MinFare { get; set; }

        /// <summary>
        /// Gets or sets the maximum fare.
        /// </summary>
        /// <value>
        /// The maximum fare.
        /// </value>
        int? MaxFare { get; set; }

        /// <summary>
        /// Gets or sets the departure date.
        /// </summary>
        /// <value>
        /// The departure date.
        /// </value>
        DateTime DepartureDate { get; set; }
    }
}