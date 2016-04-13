using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class FareInfoTPAExtensions
    {
        [DataMember(Name = "SeatsRemaining", EmitDefaultValue = false)]
        public SeatsRemaining SeatsRemaining { get; set; }

        [DataMember(Name = "Cabin", EmitDefaultValue = false)]
        public Cabin Cabin { get; set; }

        [DataMember(Name = "Meal", EmitDefaultValue = false)]
        public Meal Meal { get; set; }
    }
}