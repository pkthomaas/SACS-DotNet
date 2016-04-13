using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.BargainFinderMax
{
    [DataContract]
    public class BargainFinderMaxRQ
    {
        [DataMember(Name = "OTA_AirLowFareSearchRQ")]
        public OTAAirLowFareSearchRQ OTAAirLowFareSearchRQ { get; set; }
    }
}