using System.Text.Json.Serialization;

namespace Herval.RPA.Sdk.Models.Responses;

public class CaptchaResponse
{
    [JsonPropertyName("data")]
    public string Token { get; set; }
}

