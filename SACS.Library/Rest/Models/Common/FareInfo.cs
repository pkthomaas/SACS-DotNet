using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SACS.Library.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class FareInfo
    {
        [JsonConverter(typeof(ObjectOrStringConverter<LowestFare>))]
        [DataMember(Name = "LowestFare", EmitDefaultValue = false)]
        public LowestFare LowestFare { get; set; }

        [DataMember(Name = "CurrencyCode", EmitDefaultValue = false)]
        public string CurrencyCode { get; set; }

        [JsonConverter(typeof(ObjectOrStringConverter<LowestFare>))]
        [DataMember(Name = "LowestNonStopFare", EmitDefaultValue = false)]
        public LowestFare LowestNonStopFare { get; set; }

        [DataMember(Name = "DepartureDateTime", EmitDefaultValue = false)]
        public DateTime? DepartureDateTime { get; set; }

        [DataMember(Name = "ReturnDateTime", EmitDefaultValue = false)]
        public DateTime? ReturnDateTime { get; set; }

        [DataMember(Name = "Links", EmitDefaultValue = false)]
        public IList<Link> Links { get; set; }

        [DataMember(Name = "FareReference", EmitDefaultValue = false)]
        public string FareReference { get; set; }

        [DataMember(Name = "TPA_Extensions", EmitDefaultValue = false)]
        public FareInfoTPAExtensions TPAExtensions { get; set; }
    }
}