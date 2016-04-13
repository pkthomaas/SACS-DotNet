using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class Location
    {
        [DataMember(Name = "LocationCode")]
        public string LocationCode { get; set; }
    }
}