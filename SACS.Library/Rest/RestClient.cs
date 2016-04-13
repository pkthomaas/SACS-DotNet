using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using SACS.Library.Configuration;

namespace SACS.Library.Rest
{
    /// <summary>
    /// The REST client.
    /// </summary>
    public class RestClient
    {
        /// <summary>
        /// The log
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(RestClient));

        /// <summary>
        /// The REST authorization manager
        /// </summary>
        private readonly RestAuthorizationManager restAuthorizationManager;

        /// <summary>
        /// The configuration provider
        /// </summary>
        private readonly IConfigProvider config;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class.
        /// </summary>
        /// <param name="restAuthorizationManager">The REST authorization manager.</param>
        /// <param name="config">The configuration provider.</param>
        public RestClient(RestAuthorizationManager restAuthorizationManager, IConfigProvider config)
        {
            this.config = config;
            this.restAuthorizationManager = restAuthorizationManager;
        }

        /// <summary>
        /// Performs a GET call asynchronously.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response (response body will be deserialized to this type).</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <returns>The HTTP response.</returns>
        public async Task<HttpResponse<TResponse>> GetAsync<TResponse>(string requestUri)
        {
            Log.Debug("GET " + requestUri);
            return await this.CallAuthRetry<TResponse>(httpClient => httpClient.GetAsync(requestUri));
        }

        /// <summary>
        /// Performs a GET call asynchronously.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response (response body will be deserialized to this type).</typeparam>
        /// <param name="path">The relative request path (service endpoint).</param>
        /// <param name="queryDictionary">The query dictionary (mapping between parameter names and values that will be placed in the query string).</param>
        /// <returns>The HTTP response containing the deserialized result.</returns>
        public async Task<HttpResponse<TResponse>> GetAsync<TResponse>(
            string path,
            IDictionary<string, string> queryDictionary)
        {
            string queryString = string.Join("&", queryDictionary.Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value)));
            string requestUri = path + "?" + queryString;
            return await this.GetAsync<TResponse>(requestUri);
        }

        /// <summary>
        /// Performs a POST call asynchronously.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request model.</typeparam>
        /// <typeparam name="TResponse">The type of the response (response body will be deserialized to this type).</typeparam>
        /// <param name="path">The relative request path (service endpoint)</param>
        /// <param name="requestModel">The request model that will be serialized to JSON and sent in request body.</param>
        /// <returns>The HTTP response containing the deserialized result.</returns>
        public async Task<HttpResponse<TResponse>> PostAsync<TRequest, TResponse>(
            string path,
            TRequest requestModel)
        {
            string json = JsonConvert.SerializeObject(requestModel);
            Log.DebugFormat("POST {0}\n{1}", path, json);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            return await this.CallAuthRetry<TResponse>(httpClient => httpClient.PostAsync(path, content));
        }

        /// <summary>
        /// Performs a HTTP call with authentication with specified number of retry attempts.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="method">The callback used to perform request.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="forceRefresh">If set to <c>true</c>, then acquire a new authentication token.</param>
        /// <returns>The HTTP response.</returns>
        private async Task<HttpResponse<TResponse>> CallAuthRetry<TResponse>(
            Func<HttpClient, Task<HttpResponseMessage>> method,
            int retryCount = 1,
            bool forceRefresh = false)
        {
            TokenHolder tokenHolder = await this.restAuthorizationManager.GetAuthorizationTokenAsync(forceRefresh);
            if (tokenHolder.IsValid)
            {
                var response = await this.Call<TResponse>(method, tokenHolder.Token);
                if (response.StatusCode == HttpStatusCode.Unauthorized && retryCount > 0)
                {
                    return await this.CallAuthRetry<TResponse>(method, retryCount - 1, true);
                }

                return response;
            }
            else
            {
                return HttpResponse<TResponse>.Fail(tokenHolder.ErrorStatusCode, tokenHolder.ErrorMessage);
            }
        }

        /// <summary>
        /// Performs a HTTP call using the specified callback and deserializes the response.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="method">The callback used to perform request.</param>
        /// <param name="authorizationToken">The authorization token.</param>
        /// <returns>The HTTP response.</returns>
        private async Task<HttpResponse<TResponse>> Call<TResponse>(
            Func<HttpClient, Task<HttpResponseMessage>> method,
            string authorizationToken = null)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.config.Environment);
                if (authorizationToken != null)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizationToken);
                }

                var response = await method(client);
                string requestUri = response.RequestMessage.RequestUri.ToString();
                if (response.IsSuccessStatusCode)
                {
                    TResponse value = await response.Content.ReadAsAsync<TResponse>();
                    return HttpResponse<TResponse>.Success(response.StatusCode, value, requestUri);
                }
                else
                {
                    string message = await response.Content.ReadAsStringAsync();
                    return HttpResponse<TResponse>.Fail(response.StatusCode, message, requestUri);
                }
            }
        }
    }
}