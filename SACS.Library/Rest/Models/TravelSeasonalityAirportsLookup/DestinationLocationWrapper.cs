using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SACS.Library.Rest.Models.TravelSeasonalityAirportsLookup
{
    [DataContract]
    public class DestinationLocationWrapper
    {
        [DataMember(Name = "DestinationLocation")]
        public DestinationLocation DestinationLocation { get; set; }
    }
}
