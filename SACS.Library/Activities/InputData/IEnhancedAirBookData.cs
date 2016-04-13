using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Library.Activities.InputData
{
    /// <summary>
    /// The data used in EnhancedAirBook calls.
    /// </summary>
    public interface IEnhancedAirBookData
    {
        /// <summary>
        /// Gets the flight segments.
        /// </summary>
        /// <value>
        /// The flight segments.
        /// </value>
        IEnumerable<IFlightSegmentData> FlightSegments { get; }
    }
}
