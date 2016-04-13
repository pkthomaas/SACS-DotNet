using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class Cabin
    {
        [DataMember(Name = "Cabin")]
        public string CabinCode { get; set; }
    }
}