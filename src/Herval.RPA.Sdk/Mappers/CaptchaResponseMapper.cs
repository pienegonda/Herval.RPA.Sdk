using Herval.RPA.Sdk.Models;

namespace Herval.RPA.Sdk.Mappers;

internal static class CaptchaResponseMapper
{
    internal static CaptchaResponse Map(CaptchaResponse captchaResponse)
    {
        if (captchaResponse == null)
            return default;

        return new CaptchaResponse
        {
            Token = captchaResponse.Token
        };
    }
}


