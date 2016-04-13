using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class TPAExtensions
    {
        [DataMember(Name = "eTicket")]
        public ETicket ETicket { get; set; }
    }
}