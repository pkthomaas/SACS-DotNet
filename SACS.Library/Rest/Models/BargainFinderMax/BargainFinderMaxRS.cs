using System.Collections.Generic;
using System.Runtime.Serialization;
using SACS.Library.Rest.Models.Common;

namespace SACS.Library.Rest.Models.BargainFinderMax
{
    [DataContract]
    public class BargainFinderMaxRS
    {
        [DataMember(Name = "OTA_AirLowFareSearchRS")]
        public OTAAirLowFareSearchRS OTAAirLowFareSearchRS { get; set; }

        [DataMember(Name = "Links")]
        public IList<Link> Links { get; set; }
    }
}