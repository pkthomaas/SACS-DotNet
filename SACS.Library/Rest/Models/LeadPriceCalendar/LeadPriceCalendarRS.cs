using System.Collections.Generic;
using System.Runtime.Serialization;
using SACS.Library.Rest.Models.Common;

namespace SACS.Library.Rest.Models.LeadPriceCalendar
{
    [DataContract]
    public class LeadPriceCalendarRS
    {
        [DataMember(Name = "OriginLocation")]
        public string OriginLocation { get; set; }

        [DataMember(Name = "DestinationLocation")]
        public string DestinationLocation { get; set; }

        [DataMember(Name = "FareInfo")]
        public IList<FareInfo> FareInfo { get; set; }

        [DataMember(Name = "Links")]
        public IList<Link> Links { get; set; }
    }
}