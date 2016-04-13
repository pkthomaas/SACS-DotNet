using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class OriginDestinationOptions
    {
        [DataMember(Name = "OriginDestinationOption")]
        public IList<OriginDestinationOption> OriginDestinationOption { get; set; }
    }
}