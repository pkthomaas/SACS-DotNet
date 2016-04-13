using Newtonsoft.Json.Linq;

namespace SACS.Library.Serialization
{
    /// <summary>
    /// An object that can be deserialized from either JSON object or string.
    /// </summary>
    public interface IObjectOrString
    {
        /// <summary>
        /// Create the object instance from string JSON token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>The created object.</returns>
        object WhenString(JToken token);
    }
}