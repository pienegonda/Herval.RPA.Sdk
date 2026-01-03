using Herval.RPA.Sdk.Enums;
using Herval.RPA.Sdk.Helpers;
using Herval.RPA.Sdk.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Herval.RPA.Sdk.Services
{
    public class WebService : IWebService
    {
        private IWebDriver _driver;

        public void IniciarBrowser(EBrowser browser, string url)
        {
            _driver?.Quit();

            _driver = browser switch
            {
                EBrowser.Chrome => new ChromeDriver(),
                EBrowser.Edge => new EdgeDriver(),
                _ => throw new ArgumentException("Navegador não suportado.")
            };

            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(url);
        }

        public void FecharBrowser()
        {
            _driver?.Quit();
            _driver = null;
        }

        private void InicializarDriverSeNecessário()
        {
            if (_driver == null)
                throw new InvalidOperationException("O browser não foi iniciado. Execute IniciarBrowser().");
        }

        private IWebElement AguardarElementoInteragivel(
            ESeletor tipo,
            string seletor,
            int timeout = 10)
        {
            InicializarDriverSeNecessário();

            var by = SeletorHelper.ConverterParaBy(tipo, seletor);
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeout));

            return wait.Until(ExpectedConditions.ElementToBeClickable(by));
        }

        public void PreencherCampoQuandoAparecer(
            ESeletor tipo, string seletor, string texto, int timeout = 10)
        {
            var elemento = AguardarElementoInteragivel(tipo, seletor, timeout);
            elemento.Clear();
            elemento.SendKeys(texto);
        }

        public void ClicarQuandoAparecer(
            ESeletor tipo,
            string seletor,
            int timeout = 10)
        {
            var elemento = AguardarElementoInteragivel(tipo, seletor, timeout);
            elemento.Click();
        }

        public void AguardarPaginaCarregar(int timeoutSegundos = 10)
        {
            InicializarDriverSeNecessário();

            var aguardar = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSegundos));

            aguardar.Until(drv =>
            {
                var readyState = ((IJavaScriptExecutor)drv)
                    .ExecuteScript("return document.readyState")
                    ?.ToString();

                return readyState == "complete";
            });
        }

        public bool AguardarElemento(
            ESeletor tipo,
            string seletor,
            int timeout = 10)
        {
            InicializarDriverSeNecessário();

            try
            {
                var by = SeletorHelper.ConverterParaBy(tipo, seletor);

                var aguardar = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeout));
                aguardar.IgnoreExceptionTypes(typeof(NoSuchElementException));

                aguardar.Until(drv => drv.FindElement(by));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AguardandoDownloadConcluir(string caminhoArquivo, int timeoutInSeconds = 60)
        {
            InicializarDriverSeNecessário();

            var aguardar = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));

            try
            {
                return aguardar.Until(drv => File.Exists(caminhoArquivo));
            }
            catch
            {
                return false;
            }
        }

        public string ObterTextoElemento(
            ESeletor tipo,
            string seletor,
            int timeout = 10)
        {
            var elemento = AguardarElementoInteragivel(tipo, seletor, timeout);
            return elemento.Text;
        }

        public bool VerificarElementoExiste(
            ESeletor tipo,
            string seletor,
            int timeout = 10)
        {
            InicializarDriverSeNecessário();

            try
            {
                var by = SeletorHelper.ConverterParaBy(tipo, seletor);

                var aguardar = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeout));
                aguardar.IgnoreExceptionTypes(typeof(NoSuchElementException));

                aguardar.Until(drv => drv.FindElement(by));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SelecionarComboBoxPorTexto(
            ESeletor tipo,
            string seletor,
            string texto,
            int timeout = 10)
        {
            var elemento = AguardarElementoInteragivel(tipo, seletor, timeout);
            var selectElement = new SelectElement(elemento);
            selectElement.SelectByText(texto);
        }

        public string ExecutarJavaScript(string script)
        {
            InicializarDriverSeNecessário();

            var jsExecutor = (IJavaScriptExecutor)_driver;
            var resultado = jsExecutor.ExecuteScript(script);

            return resultado?.ToString() ?? string.Empty;
        }

        public string ObterHtmlElemento(ESeletor tipo, string seletor, int timeout = 10)
        {
            InicializarDriverSeNecessário();

            var elemento = AguardarElementoInteragivel(tipo, seletor, timeout);
            return elemento.GetAttribute("outerHTML");
        }

        public string ObterUrlAtual()
        {
            InicializarDriverSeNecessário();

            return _driver.Url;
        }

        public void InjetarTokenNaPagina(ETipoCaptcha tipoCaptcha, string token)
        {
            switch (tipoCaptcha)
            {
                case ETipoCaptcha.Recaptcha:
                    ExecutarJavaScript($"document.getElementById('g-recaptcha-response').innerHTML='{token}';");
                    break;

                case ETipoCaptcha.RecaptchaInvisible:
                    var existeElemento = AguardarElemento(ESeletor.Id, "g-recaptcha-response", 1);
                    if (existeElemento)
                    {
                        ExecutarJavaScript($"document.getElementById('g-recaptcha-response').innerHTML='{token}';");
                    }
                    else
                    {
                        ExecutarJavaScript($"document.getElementsByClassName('g-recaptcha-response')[0].innerHTML='{token}';");
                    }
                    break;

                case ETipoCaptcha.HCaptcha:
                    ExecutarJavaScript($"document.getElementsByName('g-recaptcha-response')[0].innerHTML='{token}';");
                    ExecutarJavaScript($"document.getElementsByName('h-captcha-response')[0].innerHTML='{token}';");
                    break;

                case ETipoCaptcha.CloudflareTurnstile:
                    ExecutarJavaScript($"document.getElementsByName('cf-turnstile-response')[0].value='{token}';");
                    break;

                case ETipoCaptcha.RecaptchaV3:
                case ETipoCaptcha.RecaptchaEnterprise:
                    break;

                default:
                    throw new NotSupportedException($"Injeção de token não suportada para '{tipoCaptcha}'");
            }
        }
    }
}
