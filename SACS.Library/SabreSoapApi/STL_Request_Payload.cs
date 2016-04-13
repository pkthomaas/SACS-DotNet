using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SACS.Library.SabreSoapApi
{
    /// <summary>
    /// The request payload.
    /// This partial class is used to handle a bug which caused the IgnoreOnError attribute not to be serialized in requests.
    /// </summary>
    public partial class STL_Request_Payload
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not the transaction should be ignored on error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the transaction is ignored on error; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property was not serialized properly.
        /// </remarks>
        [XmlAttribute(AttributeName = "IgnoreOnError")]
        public bool IgnoreOnError2 { get; set; }
    }
}
