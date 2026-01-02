using System.Text.Json.Serialization;

namespace Herval.RPA.Sdk.Models;

public class CaptchaResponse
{
    [JsonPropertyName("data")]
    public string Token { get; set; }
}

public class TaskCaptchaResponse
{
    public string TaskId { get; set; }
}

public class TaskResultResponse
{
    public string Status { get; set; }
    public TaskSolution Solution { get; set; }
}

public class TaskSolution
{
    public string Text { get; set; }
}
