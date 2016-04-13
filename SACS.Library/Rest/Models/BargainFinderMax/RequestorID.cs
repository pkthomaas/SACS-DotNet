using System.Runtime.Serialization;
using SACS.Library.Rest.Models.Common;

namespace SACS.Library.Rest.Models.BargainFinderMax
{
    [DataContract]
    public class RequestorID
    {
        [DataMember(Name = "CompanyName")]
        public CompanyCode CompanyName { get; set; }

        [DataMember(Name = "ID")]
        public string ID { get; set; }

        [DataMember(Name = "Type")]
        public string Type { get; set; }
    }
}