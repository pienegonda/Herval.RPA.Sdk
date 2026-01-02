using System.Text.Json.Serialization;

namespace Herval.RPA.Sdk.Models.Requests;

internal class CaptchaRequest
{
    [JsonPropertyName("siteKey")]
    public string ChaveSite { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}

