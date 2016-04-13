using System.Collections.Generic;
using System.Runtime.Serialization;
using SACS.Library.Rest.Models.Common;

namespace SACS.Library.Rest.Models.BargainFinderMax
{
    [DataContract]
    public class AirTravelerAvail
    {
        [DataMember(Name = "PassengerTypeQuantity")]
        public IList<PassengerTypeQuantity> PassengerTypeQuantity { get; set; }
    }
}