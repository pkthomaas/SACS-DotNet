using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class FareInfos
    {
        [DataMember(Name = "FareInfo")]
        public IList<FareInfo> FareInfo { get; set; }
    }
}