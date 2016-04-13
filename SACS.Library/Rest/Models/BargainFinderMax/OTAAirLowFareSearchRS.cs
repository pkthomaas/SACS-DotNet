using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.BargainFinderMax
{
    [DataContract]
    public class OTAAirLowFareSearchRS
    {
        [DataMember(Name = "PricedItinCount")]
        public int PricedItinCount { get; set; }

        [DataMember(Name = "BrandedOneWayItinCount")]
        public int BrandedOneWayItinCount { get; set; }

        [DataMember(Name = "SimpleOneWayItinCount")]
        public int SimpleOneWayItinCount { get; set; }

        [DataMember(Name = "DepartedItinCount")]
        public int DepartedItinCount { get; set; }

        [DataMember(Name = "SoldOutItinCount")]
        public int SoldOutItinCount { get; set; }

        [DataMember(Name = "AvailableItinCount")]
        public int AvailableItinCount { get; set; }

        [DataMember(Name = "Version")]
        public string Version { get; set; }

        [DataMember(Name = "Success")]
        public Success Success { get; set; }

        [DataMember(Name = "Warnings")]
        public Warnings Warnings { get; set; }

        [DataMember(Name = "PricedItineraries")]
        public PricedItineraries PricedItineraries { get; set; }
    }
}