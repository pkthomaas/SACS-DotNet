using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class PassengerFare
    {
        [DataMember(Name = "FareConstruction")]
        public Fare FareConstruction { get; set; }

        [DataMember(Name = "TotalFare")]
        public Fare TotalFare { get; set; }

        [DataMember(Name = "Taxes")]
        public Taxes Taxes { get; set; }

        [DataMember(Name = "BaseFare")]
        public Fare BaseFare { get; set; }

        [DataMember(Name = "EquivFare")]
        public Fare EquivFare { get; set; }
    }
}