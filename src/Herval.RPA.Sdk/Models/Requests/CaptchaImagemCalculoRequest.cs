using System.Text.Json.Serialization;

namespace Herval.RPA.Sdk.Models.Requests;

internal class CaptchaImagemCalculoRequest
{
    [JsonPropertyName("clientKey")]
    public string ClientKey { get; set; }

    [JsonPropertyName("task")]
    public CaptchaTaskRequest Task { get; set; }
}
