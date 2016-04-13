using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class SeatsRemaining
    {
        [DataMember(Name = "Number")]
        public int Number { get; set; }

        [DataMember(Name = "BelowMin")]
        public bool BelowMin { get; set; }
    }
}