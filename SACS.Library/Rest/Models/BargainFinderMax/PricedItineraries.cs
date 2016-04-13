using System.Collections.Generic;
using System.Runtime.Serialization;
using SACS.Library.Rest.Models.Common;

namespace SACS.Library.Rest.Models.BargainFinderMax
{
    [DataContract]
    public class PricedItineraries
    {
        [DataMember(Name = "PricedItinerary")]
        public IList<PricedItinerary> PricedItinerary { get; set; }
    }
}