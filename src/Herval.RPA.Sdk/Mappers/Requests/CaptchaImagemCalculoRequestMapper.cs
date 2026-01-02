using Herval.RPA.Sdk.Models.Requests;

namespace Herval.RPA.Sdk.Mappers.Requests;

internal static class CaptchaImagemCalculoRequestMapper
{
    internal static CaptchaImagemCalculoRequest Map(
        string clientKey, 
        string type, 
        string body, 
        bool math, 
        int numeric)
    {
        if (clientKey is null)
            return default;

        return new CaptchaImagemCalculoRequest
        {
            ClientKey = clientKey,
            Task = new CaptchaTaskRequest
            {
                Tipo = type,
                CorpoRequisicao = body,
                PrecisaCalcular = math,
                Numerico = numeric
            }
        };
    }
}
