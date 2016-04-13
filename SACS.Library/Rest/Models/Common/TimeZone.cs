using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class TimeZone
    {
        [DataMember(Name = "GMTOffset")]
        public double GMTOffset { get; set; }
    }
}