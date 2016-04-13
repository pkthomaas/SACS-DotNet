using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.BargainFinderMax
{
    [DataContract]
    public class BargainFinderMaxTPAExtensions
    {
        [DataMember(Name = "IntelliSellTransaction")]
        public IntelliSellTransaction IntelliSellTransaction { get; set; }
    }
}