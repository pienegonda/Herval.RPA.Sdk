using Herval.RPA.Sdk.Enums;

namespace Herval.RPA.Sdk.Interfaces;

public interface ICaptchaService
{
    Task<string> ResolverRecaptcha(string url, string siteKey);
    Task<string> ResolverRecaptchaInvisible(string url, string xpathCaptcha);
    Task<string> ResolverRecaptchaV3(string url, string xpathCaptcha, string action = "");
    Task<string> ResolverRecaptchaEnterprise(string url, string xpathCaptcha, string action = "");
    Task<string> ResolverHCaptcha(string url, string siteKey);
    Task<string> ResolverImageCaptcha(string imagePath);
    Task<string> ResolverCloudflareTurnstile(string url, string siteKey);
    Task<string> ResolverImageCalculoCaptcha(string imagePath);
    Task<bool> ResolverCaptchaPorTipo(ETipoCaptcha tipoCaptcha, string url, string xpathCaptcha = "", string action = "", string imagePath = "");
}
