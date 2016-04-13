using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class Taxes
    {
        [DataMember(Name = "TotalTax")]
        public Fare TotalTax { get; set; }

        [DataMember(Name = "Tax")]
        public IList<Tax> Tax { get; set; }
    }
}