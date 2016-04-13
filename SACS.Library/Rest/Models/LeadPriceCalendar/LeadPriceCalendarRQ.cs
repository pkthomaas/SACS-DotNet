using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.LeadPriceCalendar
{
    [DataContract]
    public class LeadPriceCalendarRQ
    {
        [DataMember(Name = "origin")]
        public string Origin { get; set; }

        [DataMember(Name = "destination")]
        public string Destination { get; set; }

        [DataMember(Name = "lengthofstay")]
        public int LengthOfStay { get; set; }

        [DataMember(Name = "departuredate")]
        public string DepartureDate { get; set; }

        [DataMember(Name = "minfare")]
        public string MinFare { get; set; }

        [DataMember(Name = "maxfare")]
        public string MaxFare { get; set; }

        [DataMember(Name = "pointofsalecountry")]
        public string PointOfSaleCountry { get; set; }
    }
}