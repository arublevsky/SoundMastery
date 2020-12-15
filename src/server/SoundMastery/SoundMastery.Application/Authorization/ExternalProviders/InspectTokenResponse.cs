using System.Text.Json.Serialization;

namespace SoundMastery.Application.Authorization.ExternalProviders
{
    public class InspectTokenResponse
    {
        [JsonPropertyName("data")]
        public InspectTokenData? Data { get; set; }
    }

    public class InspectTokenData
    {
        [JsonPropertyName("app_id")]
        public string AppId { get; set; } = string.Empty;

        [JsonPropertyName("is_valid")]
        public bool IsValid { get; set; }
    }
}
