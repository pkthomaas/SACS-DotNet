using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using SACS.Library.Workflow;

namespace SACS.Library.Soap
{
    /// <summary>
    /// Extension methods for <see cref="SharedContext"/> for handling SOAP results.
    /// </summary>
    public static class SharedContextExtensions
    {
        /// <summary>
        /// Adds the result serialized to XML.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="sharedContext">The shared context.</param>
        /// <param name="sharedContextKey">The shared context key.</param>
        /// <param name="result">The result.</param>
        public static void AddSerializedResultXML<TResult>(this SharedContext sharedContext, string sharedContextKey, TResult result)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TResult));
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true
            };
            using (StringWriter writer = new StringWriter())
            using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
            {
                serializer.Serialize(xmlWriter, result);
                string resultXML = writer.ToString();
                sharedContext.AddResult(sharedContextKey, resultXML);
            }
        }
    }
}
