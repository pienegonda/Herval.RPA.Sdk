using System.Text.Json.Serialization;

namespace Herval.RPA.Sdk.Models.Requests;

internal class RecaptchaRequest
{
    [JsonPropertyName("siteKey")] 
    public string ChaveSite { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
    
    [JsonPropertyName("action")]
    public string Action { get; set; }

    [JsonPropertyName("enterprise")]
    public int Enterprise { get; set; } = 1;
}
