using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using SACS.Library.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class LowestFare : IObjectOrString
    {
        [DataMember(Name = "AirlineCodes")]
        public IList<string> AirlineCodes { get; set; }

        [DataMember(Name = "Fare")]
        public decimal Fare { get; set; }

        public object WhenString(JToken token)
        {
            return new LowestFare
            {
                AirlineCodes = new List<string>()
            };
        }
    }
}