using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SACS.Library.SabreSoapApi
{
    /// <summary>
    /// The unique identifier. 
    /// This partial class is used to handle this bug:
    /// <!-- http://stackoverflow.com/questions/10109689/xsd-exe-missing-nested-properties -->
    /// (property is missing in generated code)
    /// </summary>
    public partial class UniqueID_Type
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        /// <remarks>
        /// This property was missing in the generated model.
        /// </remarks>
        [XmlAttribute]
        public string ID { get; set; }
    }
}
