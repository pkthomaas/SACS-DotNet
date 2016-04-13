using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.BargainFinderMax
{
    [DataContract]
    public class OTAAirLowFareSearchRQ
    {
        [DataMember(Name = "OriginDestinationInformation")]
        public IList<OriginDestinationInformation> OriginDestinationInformation { get; set; }

        [DataMember(Name = "POS")]
        public POS POS { get; set; }

        [DataMember(Name = "TPA_Extensions")]
        public BargainFinderMaxTPAExtensions TPAExtensions { get; set; }

        [DataMember(Name = "TravelPreferences")]
        public TravelPreferences TravelPreferences { get; set; }

        [DataMember(Name = "TravelerInfoSummary")]
        public TravelerInfoSummary TravelerInfoSummary { get; set; }
    }
}