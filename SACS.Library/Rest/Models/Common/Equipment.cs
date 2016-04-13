using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class Equipment
    {
        [DataMember(Name = "AirEquipType")]
        public string AirEquipType { get; set; }
    }
}