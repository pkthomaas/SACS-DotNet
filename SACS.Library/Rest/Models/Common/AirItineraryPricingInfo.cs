using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class AirItineraryPricingInfo
    {
        [DataMember(Name = "PTC_FareBreakdowns")]
        public PTCFareBreakdowns PTCFareBreakdowns { get; set; }

        [DataMember(Name = "FareInfos")]
        public FareInfos FareInfos { get; set; }

        [DataMember(Name = "TPA_Extensions")]
        public ItineraryTPAExtensions TPAExtensions { get; set; }

        [DataMember(Name = "ItinTotalFare")]
        public PassengerFare ItinTotalFare { get; set; }
    }
}