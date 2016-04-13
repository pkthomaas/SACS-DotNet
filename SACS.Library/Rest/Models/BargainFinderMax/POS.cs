using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.BargainFinderMax
{
    [DataContract]
    public class POS
    {
        [DataMember(Name = "Source")]
        public IList<Source> Source { get; set; }
    }
}