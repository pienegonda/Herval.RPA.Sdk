using System.Text.Json.Serialization;

namespace Herval.RPA.Sdk.Models.Responses;

internal class CaptchaImagemCalculoResponse
{
    [JsonPropertyName("taskId")]
    public int TaskId { get; set; }
}
