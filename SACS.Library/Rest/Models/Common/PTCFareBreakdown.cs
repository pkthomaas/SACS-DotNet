using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class PTCFareBreakdown
    {
        [DataMember(Name = "FareBasisCodes")]
        public FareBasisCodes FareBasisCodes { get; set; }

        [DataMember(Name = "PassengerTypeQuantity")]
        public PassengerTypeQuantity PassengerTypeQuantity { get; set; }

        [DataMember(Name = "PassengerFare")]
        public PassengerFare PassengerFare { get; set; }
    }
}