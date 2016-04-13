using System;

namespace SACS.Library.Activities.InputData
{
    /// <summary>
    /// The input data for the <see cref="InstaFlightsActivity"/> activity.
    /// </summary>
    public interface IInstaFlightsData
    {
        /// <summary>
        /// Gets the origin.
        /// </summary>
        /// <value>
        /// The origin.
        /// </value>
        string Origin { get; }

        /// <summary>
        /// Gets the destination.
        /// </summary>
        /// <value>
        /// The destination.
        /// </value>
        string Destination { get; }

        /// <summary>
        /// Gets the departure date.
        /// </summary>
        /// <value>
        /// The departure date.
        /// </value>
        DateTime DepartureDate { get; }

        /// <summary>
        /// Gets the return date.
        /// </summary>
        /// <value>
        /// The return date.
        /// </value>
        DateTime ReturnDate { get; }
    }
}