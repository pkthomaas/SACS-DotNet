using System.Net;
using Newtonsoft.Json;
using SACS.Library.Workflow;

namespace SACS.Library.Rest
{
    /// <summary>
    /// The extension methods for <see cref="SharedContext"/>
    /// </summary>
    public static class SharedContextExtensions
    {
        /// <summary>
        /// Adds the REST result (either deserialized model or error message) to shared context.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="sharedContext">The shared context.</param>
        /// <param name="key">The shared context key.</param>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="allowedNotFound">if set to <c>true</c>, then 404 Not Found is not treated as error.</param>
        /// <param name="jsonKey">The shared context key under which the reformatted JSON response will be placed.</param>
        public static void AddRestResult<TResult>(
            this SharedContext sharedContext, 
            string key, 
            HttpResponse<TResult> httpResponse, 
            bool allowedNotFound = false,
            string jsonKey = null)
        {
            if (httpResponse.IsSuccess || (allowedNotFound && httpResponse.StatusCode == HttpStatusCode.NotFound))
            {
                sharedContext.AddResult(key, httpResponse.Value);
                sharedContext.AddSerializedResultJson(jsonKey, httpResponse.Value);
            }
            else
            {
                sharedContext.AddResult(key, httpResponse.Message);
                sharedContext.IsFaulty = true;
            }
        }

        /// <summary>
        /// Adds the result serialized as JSON to the shared context.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="sharedContext">The shared context.</param>
        /// <param name="key">The key.</param>
        /// <param name="result">The result.</param>
        public static void AddSerializedResultJson<TResult>(this SharedContext sharedContext, string key, TResult result)
        {
            string json = JsonConvert.SerializeObject(result, Formatting.Indented);
            sharedContext.AddResult(key, json);
        }
    }
}