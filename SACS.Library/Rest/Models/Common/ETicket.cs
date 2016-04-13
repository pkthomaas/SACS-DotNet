using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class ETicket
    {
        [DataMember(Name = "Ind")]
        public bool Ind { get; set; }
    }
}