using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class ValidatingCarrier
    {
        [DataMember(Name = "SettlementMethod", EmitDefaultValue = false)]
        public string SettlementMethod { get; set; }

        [DataMember(Name = "NewVcxProcess", EmitDefaultValue = false)]
        public bool NewVcxProcess { get; set; }

        [DataMember(Name = "Default", EmitDefaultValue = false)]
        public string Default { get; set; }

        [DataMember(Name = "Code", EmitDefaultValue = false)]
        public string Code { get; set; }
    }
}