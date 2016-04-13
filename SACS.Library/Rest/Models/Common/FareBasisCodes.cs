using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class FareBasisCodes
    {
        [DataMember(Name = "FareBasisCode")]
        public IList<FareBasisCode> FareBasisCode { get; set; }
    }
}