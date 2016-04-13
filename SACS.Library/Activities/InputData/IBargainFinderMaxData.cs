using System;

namespace SACS.Library.Activities.InputData
{
    /// <summary>
    /// The input data for the <see cref="BargainFinderMaxActivity"/> activity.
    /// </summary>
    public interface IBargainFinderMaxData
    {
        /// <summary>
        /// Gets the departure date.
        /// </summary>
        /// <value>
        /// The departure date.
        /// </value>
        DateTime DepartureDate { get; }

        /// <summary>
        /// Gets the origin airport code.
        /// </summary>
        /// <value>
        /// The origin airport code.
        /// </value>
        string OriginAirportCode { get; }

        /// <summary>
        /// Gets the destination airport code.
        /// </summary>
        /// <value>
        /// The destination airport code.
        /// </value>
        string DestinationAirportCode { get; }

        /// <summary>
        /// Gets the RPH.
        /// </summary>
        /// <value>
        /// The RPH.
        /// </value>
        string RPH { get; }

        /// <summary>
        /// Gets the requestor identifier.
        /// </summary>
        /// <value>
        /// The requestor identifier.
        /// </value>
        string RequestorID { get; }

        /// <summary>
        /// Gets the type of the requestor.
        /// </summary>
        /// <value>
        /// The type of the requestor.
        /// </value>
        string RequestorType { get; }

        /// <summary>
        /// Gets the requestor company code.
        /// </summary>
        /// <value>
        /// The requestor company code.
        /// </value>
        string RequestorCompanyCode { get; }

        /// <summary>
        /// Gets the passenger type code.
        /// </summary>
        /// <value>
        /// The passenger type code.
        /// </value>
        string PassengerTypeCode { get; }

        /// <summary>
        /// Gets the number of passengers.
        /// </summary>
        /// <value>
        /// The number of passengers.
        /// </value>
        int NumberOfPassengers { get; }

        /// <summary>
        /// Gets the type of the request (should always be 50TINS).
        /// </summary>
        /// <value>
        /// The type of the request.
        /// </value>
        string RequestType { get; }
    }
}