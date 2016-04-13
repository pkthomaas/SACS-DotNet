using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.BargainFinderMax
{
    [DataContract]
    public class Warnings
    {
        [DataMember(Name = "Warning")]
        public IList<Warning> Warning { get; set; }
    }
}