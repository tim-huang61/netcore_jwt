using System;
using System.Text.Json.Serialization;

namespace WebAPI.JWT.Models
{
    public class JwtToken
    {
        [JsonPropertyName("access_token")] 
        public string Token { get; set; }

        [JsonPropertyName("expires_in")] 
        public DateTime ExpiredTime { get; set; }

        [JsonPropertyName("token_type")] 
        public string Type => "Bearer";
    }
}