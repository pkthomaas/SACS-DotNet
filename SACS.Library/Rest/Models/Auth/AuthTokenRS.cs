using System.Runtime.Serialization;

namespace SACS.Library.Rest.Models.Auth
{
    [DataContract]
    public class AuthTokenRS
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }

        [DataMember(Name = "expires_in")]
        public int ExpiresIn { get; set; }
    }
}