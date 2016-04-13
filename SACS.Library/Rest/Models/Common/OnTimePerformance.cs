using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class OnTimePerformance
    {
        [DataMember(Name = "Level")]
        public string Level { get; set; }
    }
}