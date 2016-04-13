using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.BargainFinderMax
{
    [DataContract]
    public class IntelliSellTransaction
    {
        [DataMember(Name = "RequestType")]
        public RequestType RequestType { get; set; }
    }
}