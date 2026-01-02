using Herval.RPA.Sdk.Enums;
using Herval.RPA.Sdk.Interfaces;
using Herval.RPA.Sdk.Mappers.Requests;
using Herval.RPA.Sdk.Mappers.Responses;
using Herval.RPA.Sdk.Models.Requests;
using Herval.RPA.Sdk.Models.Responses;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace Herval.RPA.Sdk.Services;

public class CaptchaService : ICaptchaService
{
    private readonly IWebService _webService;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CaptchaService(
        IWebService webService,
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _webService = webService;
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<bool> ResolverCaptchaPorTipo(ETipoCaptcha tipoCaptcha, string url, string xpathCaptcha = "", string action = "", string imagePath = "")
    {
        try
        {
            var urlAtual = string.IsNullOrEmpty(url) ? _webService.ObterUrlAtual() : url;

            string token = tipoCaptcha switch
            {
                ETipoCaptcha.Recaptcha => await ResolverRecaptcha(urlAtual, ExtrairSiteKeyDaPagina("g-recaptcha")),
                ETipoCaptcha.RecaptchaInvisible => await ResolverRecaptchaInvisible(urlAtual, xpathCaptcha),
                ETipoCaptcha.RecaptchaV3 => await ResolverRecaptchaV3(urlAtual, xpathCaptcha, action),
                ETipoCaptcha.RecaptchaEnterprise => await ResolverRecaptchaEnterprise(urlAtual, xpathCaptcha, action),
                ETipoCaptcha.HCaptcha => await ResolverHCaptcha(urlAtual, ExtrairSiteKeyDaPagina("h-captcha")),
                ETipoCaptcha.Image => await ResolverImageCaptcha(imagePath),
                ETipoCaptcha.CloudflareTurnstile => await ResolverCloudflareTurnstile(urlAtual, ExtrairSiteKeyDaPagina("cf-turnstile")),
                ETipoCaptcha.ImageCalculo => await ResolverImageCalculoCaptcha(imagePath),
                _ => throw new NotSupportedException($"Tipo de captcha '{tipoCaptcha}' não suportado")
            };

            _webService.InjetarTokenNaPagina(tipoCaptcha, token);
            return true;
        }
        catch(Exception ex)
        {
            throw new Exception($"Erro ao resolver captcha: {ex.Message}", ex);
        }
    }

    public async Task<string> ResolverRecaptcha(string url, string siteKey)
    {
        try
        {
            var payload = CaptchaRequestMapper.Map(url, siteKey);

            var response = await _httpClient.PostAsJsonAsync($"{_configuration["Captcha:ApiBaseUrl"]}/recaptcha", payload);
            var captchaResponse = await TratamentoRetornoAsync<CaptchaResponse>(response);

            if (captchaResponse is null)
                return default;

            var token = CaptchaResponseMapper.Map(captchaResponse);
            return token.Token;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao resolver reCAPTCHA: {ex.Message}", ex);
        }
    }

    public async Task<string> ResolverRecaptchaInvisible(string url, string xpathCaptcha)
    {
        try
        {
            var elementoHTML = _webService.ObterHtmlElemento(ESeletor.XPath, xpathCaptcha);
            var siteKey = ExtrairSiteKey(elementoHTML);

            var payload = CaptchaRequestMapper.Map(url, siteKey);

            var response = await _httpClient.PostAsJsonAsync($"{_configuration["Captcha:ApiBaseUrl"]}/recaptcha", payload);
            var captchaResponse = await TratamentoRetornoAsync<CaptchaResponse>(response);

            if (captchaResponse is null)
                return default;

            var token = CaptchaResponseMapper.Map(captchaResponse);
            return token.Token;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao resolver reCAPTCHA invisible: {ex.Message}", ex);
        }
    }

    public async Task<string> ResolverRecaptchaV3(string url, string xpathCaptcha, string action = "")
    {
        try
        {
            var elementoHTML = _webService.ObterHtmlElemento(ESeletor.XPath, xpathCaptcha);
            var siteKey = ExtrairSiteKey(elementoHTML);

            var payload = RecaptchaRequestMapper.Map(url, siteKey, action, 0);

            var response = await _httpClient.PostAsJsonAsync($"{_configuration["Captcha:ApiBaseUrl"]}/recaptchaV3", payload);
            var captchaResponse = await TratamentoRetornoAsync<CaptchaResponse>(response);

            if (captchaResponse == null)
                return default;

            var token = CaptchaResponseMapper.Map(captchaResponse);
            return token.Token;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao resolver reCAPTCHA V3: {ex.Message}", ex);
        }
    }

    public async Task<string> ResolverRecaptchaEnterprise(string url, string xpathCaptcha, string action = "")
    {
        try
        {
            var elementoHTML = _webService.ObterHtmlElemento(ESeletor.XPath, xpathCaptcha);
            var siteKey = ExtrairSiteKey(elementoHTML);

            var payload = RecaptchaRequestMapper.Map(url, siteKey, action, 1);

            var response = await _httpClient.PostAsJsonAsync($"{_configuration["Captcha:ApiBaseUrl"]}/recaptcha-enterprise", payload);
            var captchaResponse = await TratamentoRetornoAsync<CaptchaResponse>(response);

            if (captchaResponse == null)
                return default;

            var token = CaptchaResponseMapper.Map(captchaResponse);
            return token.Token;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao resolver reCAPTCHA Enterprise: {ex.Message}", ex);
        }
    }

    public async Task<string> ResolverHCaptcha(string url, string siteKey)
    {
        try
        {
            var payload = CaptchaRequestMapper.Map(url, siteKey);

            var response = await _httpClient.PostAsJsonAsync($"{_configuration["Captcha:ApiBaseUrl"]}/hcaptcha", payload);
            var captchaResponse = await TratamentoRetornoAsync<CaptchaResponse>(response);

            if (captchaResponse == null)
                return default;

            var token = CaptchaResponseMapper.Map(captchaResponse);
            return token.Token;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao resolver hCAPTCHA: {ex.Message}", ex);
        }
    }

    public async Task<string> ResolverImageCaptcha(string imagePath)
    {
        try
        {
            var payload = new
            {
                image = imagePath.Replace("\\", "\\\\")
            };

            var response = await _httpClient.PostAsJsonAsync($"{_configuration["Captcha:ApiBaseUrl"]}/image", payload);
            var captchaResponse = await TratamentoRetornoAsync<CaptchaResponse>(response);

            if (captchaResponse == null)
                return default;

            var token = CaptchaResponseMapper.Map(captchaResponse);
            return token.Token;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao resolver CAPTCHA de imagem: {ex.Message}", ex);
        }
    }

    public async Task<string> ResolverCloudflareTurnstile(string url, string siteKey)
    {
        try
        {
            var payload = CaptchaRequestMapper.Map(url, siteKey);

            var response = await _httpClient.PostAsJsonAsync($"{_configuration["Captcha:ApiBaseUrl"]}/cloudflare-turnstile", payload);
            var captchaResponse = await TratamentoRetornoAsync<CaptchaResponse>(response);

            if (captchaResponse == null)
                return default;

            var token = CaptchaResponseMapper.Map(captchaResponse);
            return token.Token;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao resolver Cloudflare Turnstile: {ex.Message}", ex);
        }
    }

    public async Task<string> ResolverImageCalculoCaptcha(string imagePath)
    {
        try
        {
            var base64Image = Convert.ToBase64String(File.ReadAllBytes(imagePath));

            var payload = CaptchaImagemCalculoRequestMapper.Map(_configuration["Captcha:ApiKey2Captcha"],"ImageToTextTask",base64Image,true,1);

            var createTaskResponse = await _httpClient.PostAsJsonAsync("https://api.2captcha.com/createTask", payload);
            var createTaskResult = await TratamentoRetornoAsync<CaptchaImagemCalculoResponse>(createTaskResponse);
            var taskId = CaptchaImagemCalculoResponseMapper.Map(createTaskResult);

            if (taskId is null)
                throw new Exception("Falha ao criar task de captcha de cálculo");
            
            await Task.Delay(10000);

            var getResultPayload = new
            {
                clientKey = _configuration["Captcha:ApiKey2Captcha"],
                taskId = taskId
            };

            var getResultResponse = await _httpClient.PostAsJsonAsync("https://api.2captcha.com/getTaskResult", getResultPayload);
            var resultObject = await TratamentoRetornoAsync<JObject>(getResultResponse);
            var token = resultObject?["solution"]?["text"]?.ToString();

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Falha ao resolver captcha de cálculo");
            }

            return token;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao resolver CAPTCHA de cálculo: {ex.Message}", ex);
        }
    }

    private string ExtrairSiteKey(string html)
    {
        // Tenta primeiro o padrão ;k= padrão da lógica LOWCODE
        var match = Regex.Match(html, ";k=[^&;]+");

        if (!match.Success)
        {
            // Tenta o padrão &k= padrão da lógica LOWCODE
            match = Regex.Match(html, "&k=[^&;]+");
        }

        if (!match.Success)
        {
            throw new Exception("Site key não encontrada no HTML");
        }

        // Remove os primeiros 3 caracteres (;k= ou &k=) padrão da lógica LOWCODE
        return match.Value.Substring(3);
    }

    private string ExtrairSiteKeyDaPagina(string className)
    {
        var script = $"return document.getElementsByClassName('{className}')[0].getAttribute('data-sitekey');";
        return _webService.ExecutarJavaScript(script);
    }

    private async Task<T> TratamentoRetornoAsync<T>(HttpResponseMessage response)
    {
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                return await response.Content.ReadFromJsonAsync<T>();

            case HttpStatusCode.UnprocessableEntity:
                throw new Exception($"Erro de validação: {response.StatusCode}");

            case HttpStatusCode.ServiceUnavailable:
                throw new Exception($"Serviço indisponível: {response.StatusCode}");

            case HttpStatusCode.NotFound:
                throw new Exception($"Recurso não encontrado: {response.StatusCode}");

            case HttpStatusCode.NoContent:
                return default;

            default:
                throw new Exception($"Erro na comunicação com a API de Captcha: {response.StatusCode}");
        }
    }

}

