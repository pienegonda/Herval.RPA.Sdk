using Herval.RPA.Sdk.Models.Responses;

namespace Herval.RPA.Sdk.Mappers.Responses;

internal static class CaptchaResponseMapper
{
    internal static CaptchaResponse Map(CaptchaResponse response)
    {
        if (response == null)
            return default;

        return new CaptchaResponse
        {
            Token = response.Token
        };
    }
}


