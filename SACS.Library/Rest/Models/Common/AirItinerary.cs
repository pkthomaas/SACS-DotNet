using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class AirItinerary
    {
        [DataMember(Name = "OriginDestinationOptions")]
        public OriginDestinationOptions OriginDestinationOptions { get; set; }

        [DataMember(Name = "DirectionInd")]
        public string DirectionInd { get; set; }
    }
}