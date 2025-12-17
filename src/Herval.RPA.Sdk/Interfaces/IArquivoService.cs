namespace Herval.RPA.Sdk.Interfaces
{
    public interface IArquivoService
    {
        void MoverArquivo(string caminhoOrigem, string caminhoDestino);
        void CopiarArquivo(string caminhoOrigem, string caminhoDestino, bool sobrescrever = false);
        void DeletarArquivo(string caminhoArquivo);
        List<string> ListarArquivosEmDiretorio(string caminhoDiretorio);
        void CriarDiretorio(string caminhoDiretorio);
        string LerArquivo(string caminhoArquivo);
        string ObterNomeArquivo(string caminhoArquivo);
        string ObterTextoPdf(string caminhoArquivo);
    }
}