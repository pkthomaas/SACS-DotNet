using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.BargainFinderMax
{
    [DataContract]
    public class TravelerInfoSummary
    {
        [DataMember(Name = "AirTravelerAvail")]
        public IList<AirTravelerAvail> AirTravelerAvail { get; set; }
    }
}