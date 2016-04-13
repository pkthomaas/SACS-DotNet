using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SACS.Library.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class PricedItinerary
    {
        [DataMember(Name = "AirItinerary")]
        public AirItinerary AirItinerary { get; set; }

        [DataMember(Name = "TPA_Extensions")]
        public ItineraryTPAExtensions TPAExtensions { get; set; }

        [DataMember(Name = "SequenceNumber")]
        public int SequenceNumber { get; set; }

        [JsonConverter(typeof(ArrayOrObjectConverter<AirItineraryPricingInfo>))]
        [DataMember(Name = "AirItineraryPricingInfo")]
        public IList<AirItineraryPricingInfo> AirItineraryPricingInfo { get; set; }

        [DataMember(Name = "TicketingInfo")]
        public TicketingInfo TicketingInfo { get; set; }
    }
}