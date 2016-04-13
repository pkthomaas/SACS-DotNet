using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.BargainFinderMax
{
    [DataContract]
    public class Source
    {
        [DataMember(Name = "RequestorID")]
        public RequestorID RequestorID { get; set; }
    }
}