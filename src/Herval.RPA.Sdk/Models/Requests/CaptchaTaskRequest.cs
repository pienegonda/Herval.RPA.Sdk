using System.Text.Json.Serialization;

namespace Herval.RPA.Sdk.Models.Requests;

internal class CaptchaTaskRequest
{
    [JsonPropertyName("type")]
    public string Tipo { get; set; }

    [JsonPropertyName("body")]
    public string CorpoRequisicao { get; set; }

    [JsonPropertyName("math")]
    public bool PrecisaCalcular { get; set; }

    [JsonPropertyName("numeric")]
    public int Numerico { get; set; }
}
