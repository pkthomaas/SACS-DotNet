using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class OperatingAirline
    {
        [DataMember(Name = "FlightNumber")]
        public int FlightNumber { get; set; }

        [DataMember(Name = "Code")]
        public string Code { get; set; }
    }
}