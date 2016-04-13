using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.BargainFinderMax
{
    [DataContract]
    public class TravelPreferencesTPAExtensions
    {
        [DataMember(Name = "NumTrips")]
        public NumTrips NumTrips { get; set; }
    }
}