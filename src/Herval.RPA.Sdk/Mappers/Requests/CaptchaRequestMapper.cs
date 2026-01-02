using Herval.RPA.Sdk.Models.Requests;

namespace Herval.RPA.Sdk.Mappers.Responses;

internal static class CaptchaRequestMapper
{
    internal static CaptchaRequest Map(string url, string siteKey)
    {
        if (url is null)
            return default;

        return new CaptchaRequest
        {
            ChaveSite = siteKey,
            Url = url
        };
    }
}
