using Herval.RPA.Sdk.Models.Requests;

namespace Herval.RPA.Sdk.Mappers.Requests;

internal static class RecaptchaRequestMapper
{
    internal static RecaptchaRequest Map(
        string url, 
        string siteKey, 
        string action, 
        int enterprise)
    {
        if (url is null)
            return default;

        return new RecaptchaRequest
        {
            ChaveSite = siteKey,
            Url = url,
            Action = action,
            Enterprise = enterprise
        };
    }
}
