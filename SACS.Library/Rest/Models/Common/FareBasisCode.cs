using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class FareBasisCode
    {
        [DataMember(Name = "BookingCode")]
        public string BookingCode { get; set; }

        [DataMember(Name = "DepartureAirportCode")]
        public string DepartureAirportCode { get; set; }

        [DataMember(Name = "AvailabilityBreak")]
        public bool AvailabilityBreak { get; set; }

        [DataMember(Name = "ArrivalAirportCode")]
        public string ArrivalAirportCode { get; set; }

        [DataMember(Name = "content")]
        public string Content { get; set; }
    }
}