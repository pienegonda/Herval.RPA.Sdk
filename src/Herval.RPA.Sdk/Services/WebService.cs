using Herval.RPA.Sdk.Enums;
using Herval.RPA.Sdk.Helpers;
using Herval.RPA.Sdk.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;

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

            var aguardar = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeout));
            aguardar.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            return aguardar.Until(drv =>
            {
                var elem = drv.FindElement(by);
                return (elem.Displayed && elem.Enabled) ? elem : null;
            });
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
                return aguardar.Until(drv => System.IO.File.Exists(caminhoArquivo));
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
    }
}
