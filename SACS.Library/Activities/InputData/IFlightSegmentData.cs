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
    public interface IFlightSegmentData
    {
        /// <summary>
        /// Gets the departure date time.
        /// </summary>
        /// <value>
        /// The departure date time.
        /// </value>
        DateTime DepartureDateTime { get; }

        /// <summary>
        /// Gets the flight number.
        /// </summary>
        /// <value>
        /// The flight number.
        /// </value>
        string FlightNumber { get; }

        /// <summary>
        /// Gets the marketing airline code.
        /// </summary>
        /// <value>
        /// The marketing airline code.
        /// </value>
        string MarketingAirlineCode { get; }

        /// <summary>
        /// Gets the destination location (airport) code.
        /// </summary>
        /// <value>
        /// The destination location code.
        /// </value>
        string DestinationLocationCode { get; }

        /// <summary>
        /// Gets the origin location (airport) code.
        /// </summary>
        /// <value>
        /// The origin location code.
        /// </value>
        string OriginLocationCode { get; }

        /// <summary>
        /// Gets the designation code (<c>ResBookDesigCode</c>).
        /// </summary>
        /// <value>
        /// The designation code.
        /// </value>
        string DesignationCode { get; }
    }
}
