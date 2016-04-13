using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SACS.Library.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class PTCFareBreakdowns
    {
        [JsonConverter(typeof(ArrayOrObjectConverter<PTCFareBreakdown>))]
        [DataMember(Name = "PTC_FareBreakdown")]
        public IList<PTCFareBreakdown> PTCFareBreakdown { get; set; }
    }
}