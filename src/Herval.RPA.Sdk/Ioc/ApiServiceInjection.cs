using Herval.RPA.Sdk.Interfaces;
using Herval.RPA.Sdk.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Herval.RPA.Sdk.Ioc;

public static class ApiServiceInjection
{
    public static IServiceCollection AddCaptchaService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ICaptchaService, CaptchaService>(client =>
        {
            var uri = configuration.GetValue<string>("2Captcha:Endpoint");
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        return services;
    }
}

