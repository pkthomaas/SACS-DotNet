using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class OriginDestinationOption
    {
        [DataMember(Name = "FlightSegment")]
        public IList<FlightSegment> FlightSegment { get; set; }

        [DataMember(Name = "ElapsedTime")]
        public int ElapsedTime { get; set; }
    }
}