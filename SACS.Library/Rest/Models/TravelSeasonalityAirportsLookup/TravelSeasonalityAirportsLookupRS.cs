using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SACS.Library.Rest.Models.Common;

namespace SACS.Library.Rest.Models.TravelSeasonalityAirportsLookup
{
    [DataContract]
    public class TravelSeasonalityAirportsLookupRS
    {
        [DataMember(Name = "DestinationLocations")]
        public IList<DestinationLocationWrapper> DestinationLocations { get; set; }

        [DataMember(Name = "Links")]
        public IList<Link> Links { get; set; }
    }
}
