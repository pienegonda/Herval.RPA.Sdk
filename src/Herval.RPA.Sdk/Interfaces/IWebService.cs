using Herval.RPA.Sdk.Enums;

namespace Herval.RPA.Sdk.Interfaces
{
    public interface IWebService
    {
        void IniciarBrowser(EBrowser browser, string url);
        void FecharBrowser();
        public void PreencherCampoQuandoAparecer(ESeletor tipo, string seletor, string texto, int timeout = 10);
        void ClicarQuandoAparecer(ESeletor tipo, string seletor, int timeout = 10);
        void AguardarPaginaCarregar(int timeoutSegundos = 10);
        bool AguardarElemento(ESeletor tipo, string seletor, int timeout = 10);
        bool AguardandoDownloadConcluir(string caminhoArquivo, int timeoutInSeconds = 60);
        string ObterTextoElemento(ESeletor tipo, string seletor, int timeout = 10);
        bool VerificarElementoExiste(ESeletor tipo, string seletor, int timeout = 10);
        void SelecionarComboBoxPorTexto(ESeletor tipo, string seletor, string texto, int timeout = 10);
    }
}