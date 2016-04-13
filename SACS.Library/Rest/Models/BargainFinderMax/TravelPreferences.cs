using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.BargainFinderMax
{
    [DataContract]
    public class TravelPreferences
    {
        [DataMember(Name = "TPA_Extensions")]
        public TravelPreferencesTPAExtensions TPAExtensions { get; set; }
    }
}