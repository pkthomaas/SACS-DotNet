using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class Tax
    {
        [DataMember(Name = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [DataMember(Name = "DecimalPlaces")]
        public int DecimalPlaces { get; set; }

        [DataMember(Name = "TaxCode")]
        public string TaxCode { get; set; }

        [DataMember(Name = "Amount")]
        public decimal Amount { get; set; }
    }
}