using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class ItineraryTPAExtensions
    {
        [DataMember(Name = "DivideInParty", EmitDefaultValue = false)]
        public DivideInParty DivideInParty { get; set; }

        [DataMember(Name = "ValidatingCarrier", EmitDefaultValue = false)]
        public ValidatingCarrier ValidatingCarrier { get; set; }
    }
}