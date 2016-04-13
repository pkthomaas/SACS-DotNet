using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class PassengerTypeQuantity
    {
        [DataMember(Name = "Quantity")]
        public int Quantity { get; set; }

        [DataMember(Name = "Code")]
        public string Code { get; set; }
    }
}