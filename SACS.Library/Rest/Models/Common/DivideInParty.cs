using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class DivideInParty
    {
        [DataMember(Name = "Indicator")]
        public bool Indicator { get; set; }
    }
}