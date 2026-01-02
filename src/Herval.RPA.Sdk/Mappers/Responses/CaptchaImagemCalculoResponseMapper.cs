using Herval.RPA.Sdk.Models.Responses;

namespace Herval.RPA.Sdk.Mappers.Responses;

internal static class CaptchaImagemCalculoResponseMapper
{
    internal static CaptchaImagemCalculoResponse Map(CaptchaImagemCalculoResponse response)
    {
        if (response == null)
            return default;

        return new CaptchaImagemCalculoResponse
        {
            TaskId = response.TaskId
        };
    }
}
