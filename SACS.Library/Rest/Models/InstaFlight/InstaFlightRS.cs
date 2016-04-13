using System.Collections.Generic;
using System.Runtime.Serialization;
using SACS.Library.Rest.Models.Common;

namespace SACS.Library.Rest.Models.InstaFlight
{
    [DataContract]
    public class InstaFlightRS
    {
        [DataMember(Name = "PricedItineraries")]
        public IList<PricedItinerary> PricedItineraries { get; set; }

        [DataMember(Name = "ReturnDateTime")]
        public string ReturnDateTime { get; set; }

        [DataMember(Name = "DepartureDateTime")]
        public string DepartureDateTime { get; set; }

        [DataMember(Name = "DestinationLocation")]
        public string DestinationLocation { get; set; }

        [DataMember(Name = "OriginLocation")]
        public string OriginLocation { get; set; }

        [DataMember(Name = "Links")]
        public IList<Link> Links { get; set; }
    }
}