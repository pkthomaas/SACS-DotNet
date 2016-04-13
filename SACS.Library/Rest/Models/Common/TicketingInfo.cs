using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class TicketingInfo
    {
        [DataMember(Name = "TicketType")]
        public string TicketType { get; set; }
    }
}