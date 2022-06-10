using Newtonsoft.Json;

namespace Cortside.MockServer.AccessControl.Models {
    public class AuthenticationResponseModel {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}
