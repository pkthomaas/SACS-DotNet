using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using log4net;
using SACS.Library.Configuration;
using SACS.Library.Rest.Models.Auth;

namespace SACS.Library.Rest
{
    /// <summary>
    /// The authorization manager for REST calls.
    /// </summary>
    public class RestAuthorizationManager
    {
        /// <summary>
        /// The authorization endpoint address
        /// </summary>
        private const string AuthorizationEndpoint = "/v2/auth/token";
        
        /// <summary>
        /// The format version (required for call)
        /// </summary>
        private const string FormatVersion = "V1";
        
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RestAuthorizationManager));

        /// <summary>
        /// The configuration provider.
        /// </summary>
        private readonly IConfigProvider config;
        
        /// <summary>
        /// The token holder.
        /// </summary>
        private TokenHolder tokenHolder = TokenHolder.Empty();

        /// <summary>
        /// Initializes a new instance of the <see cref="RestAuthorizationManager"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public RestAuthorizationManager(IConfigProvider config)
        {
            this.config = config;
        }

        /// <summary>
        /// Performs the authorization call asynchronously.
        /// </summary>
        /// <param name="credentials">The credentials string.</param>
        /// <returns>The HTTP response with authorization result model.</returns>
        public async Task<HttpResponse<AuthTokenRS>> AuthorizeAsync(string credentials)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.config.Environment);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

                var args = new Dictionary<string, string>();
                args.Add("grant_type", "client_credentials");
                var content = new FormUrlEncodedContent(args);
                Logger.DebugFormat("POST {0}\n{1}", AuthorizationEndpoint, await content.ReadAsStringAsync());

                var response = await client.PostAsync(AuthorizationEndpoint, content);
                string requestUri = response.RequestMessage.RequestUri.ToString();
                if (response.IsSuccessStatusCode)
                {
                    AuthTokenRS value = await response.Content.ReadAsAsync<AuthTokenRS>();
                    return HttpResponse<AuthTokenRS>.Success(response.StatusCode, value, requestUri);
                }
                else
                {
                    return HttpResponse<AuthTokenRS>.Fail(response.StatusCode, await response.Content.ReadAsStringAsync(), requestUri);
                }
            }
        }

        /// <summary>
        /// Creates the credentials string.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="group">The group.</param>
        /// <param name="secret">The secret.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="formatVersion">The format version.</param>
        /// <returns>The credentials string.</returns>
        public string CreateCredentialsString(string userId, string group, string secret, string domain = "AA", string formatVersion = "V1")
        {
            string clientId = string.Format("{0}:{1}:{2}:{3}", formatVersion, userId, group, domain);
            return Base64Encode(Base64Encode(clientId) + ":" + Base64Encode(secret));
        }

        /// <summary>
        /// Gets the authorization token. If token is no longer valid, performs the authorization call asynchronously.
        /// </summary>
        /// <param name="forceRefresh">if set to <c>true</c>, then discard current token and authorize again.</param>
        /// <returns>The token holder.</returns>
        public async Task<TokenHolder> GetAuthorizationTokenAsync(bool forceRefresh = false)
        {
            if (forceRefresh || this.tokenHolder == null || this.tokenHolder.Token == null || this.tokenHolder.ExpirationDate <= DateTime.Now)
            {
                string userId = this.config.UserId;
                string group = this.config.Group;
                string secret = this.config.ClientSecret;
                string domain = this.config.Domain;
                string formatVersion = FormatVersion;
                string clientId = this.CreateCredentialsString(userId, group, secret, domain, formatVersion);
                var response = await this.AuthorizeAsync(clientId);
                if (response.IsSuccess)
                {
                    var value = response.Value;
                    this.tokenHolder = TokenHolder.Valid(value.AccessToken, value.ExpiresIn);
                }
                else
                {
                    Logger.DebugFormat("Authorization failed.\nStatus code: {0}\nMessage:\n{1}", response.StatusCode, response.Message);
                    this.tokenHolder = TokenHolder.Invalid(response.StatusCode, response.Message);
                }
            }

            return this.tokenHolder;
        }

        /// <summary>
        /// Encodes the string in base64.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns>The string in base64.</returns>
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}