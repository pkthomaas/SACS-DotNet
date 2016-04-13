using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SACS.Library.Rest.Models.TravelSeasonalityAirportsLookup
{
    [DataContract]
    public class DestinationLocation
    {
        [DataMember(Name = "AirportCode")]
        public string AirportCode { get; set; }

        [DataMember(Name = "AirportName")]
        public string AirportName { get; set; }

        [DataMember(Name = "CityName")]
        public string CityName { get; set; }

        [DataMember(Name = "CountryCode")]
        public string CountryCode { get; set; }

        [DataMember(Name = "CountryName")]
        public string CountryName { get; set; }

        [DataMember(Name = "RegionName")]
        public string RegionName { get; set; }
    }
}
