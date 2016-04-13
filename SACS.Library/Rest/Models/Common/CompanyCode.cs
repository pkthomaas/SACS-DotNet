using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class CompanyCode
    {
        [DataMember(Name = "Code")]
        public string Code { get; set; }
    }
}