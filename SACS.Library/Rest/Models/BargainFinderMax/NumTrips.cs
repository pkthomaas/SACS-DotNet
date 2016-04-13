using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.BargainFinderMax
{
    [DataContract]
    public class NumTrips
    {
        [DataMember(Name = "Number")]
        public int Number { get; set; }
    }
}