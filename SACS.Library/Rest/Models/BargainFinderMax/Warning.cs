using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.BargainFinderMax
{
    [DataContract]
    public class Warning
    {
        [DataMember(Name = "Type")]
        public string Type { get; set; }

        [DataMember(Name = "ShortText")]
        public string ShortText { get; set; }

        [DataMember(Name = "Code")]
        public string Code { get; set; }

        [DataMember(Name = "MessageClass")]
        public string MessageClass { get; set; }

        [DataMember(Name = "content")]
        public string Content { get; set; }
    }
}