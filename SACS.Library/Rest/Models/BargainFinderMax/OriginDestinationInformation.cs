using System;
using System.Runtime.Serialization;
using SACS.Library.Rest.Models.Common;

namespace SACS.Library.Rest.Models.BargainFinderMax
{
    [DataContract]
    public class OriginDestinationInformation
    {
        [DataMember(Name = "DepartureDateTime")]
        public DateTime DepartureDateTime { get; set; }

        [DataMember(Name = "DestinationLocation")]
        public Location DestinationLocation { get; set; }

        [DataMember(Name = "OriginLocation")]
        public Location OriginLocation { get; set; }

        [DataMember(Name = "RPH")]
        public string RPH { get; set; }
    }
}