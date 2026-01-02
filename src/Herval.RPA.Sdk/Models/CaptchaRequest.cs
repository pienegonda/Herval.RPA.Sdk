namespace Herval.RPA.Sdk.Models;

public class CaptchaRequest
{
    public string SiteKey { get; set; }
    public string Url { get; set; }
    public string Action { get; set; }
    public int Enterprise { get; set; }
}

public class ImageCaptchaRequest
{
    public string Image { get; set; }
}

public class TaskCaptchaRequest
{
    public string ClientKey { get; set; }
    public TaskConfig Task { get; set; }
}

public class TaskConfig
{
    public string Type { get; set; }
    public string Body { get; set; }
    public bool Math { get; set; }
    public int Numeric { get; set; }
}

public class TaskResultRequest
{
    public string ClientKey { get; set; }
    public string TaskId { get; set; }
}
