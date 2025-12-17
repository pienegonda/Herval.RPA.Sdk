using System.Text;
using Herval.RPA.Sdk.Interfaces;
using OfficeOpenXml;
using UglyToad.PdfPig;

namespace Herval.RPA.Sdk.Services
{
    public class ArquivoService : IArquivoService
    {
        public void MoverArquivo(string caminhoOrigem, string caminhoDestino)
        {
            if (!File.Exists(caminhoOrigem))
                throw new FileNotFoundException("Arquivo de origem não encontrado.", caminhoOrigem);

            File.Move(caminhoOrigem, caminhoDestino);
        }

        public void CopiarArquivo(string caminhoOrigem, string caminhoDestino, bool sobrescrever = false)
        {
            if (!File.Exists(caminhoOrigem))
                throw new FileNotFoundException("Arquivo de origem não encontrado.", caminhoOrigem);

            File.Copy(caminhoOrigem, caminhoDestino, sobrescrever);
        }

        public void DeletarArquivo(string caminhoArquivo)
        {
            if (!File.Exists(caminhoArquivo))
                throw new FileNotFoundException("Arquivo não encontrado.", caminhoArquivo);

            File.Delete(caminhoArquivo);
        }

        public List<string> ListarArquivosEmDiretorio(string caminhoDiretorio)
        {
            if (!Directory.Exists(caminhoDiretorio))
                throw new DirectoryNotFoundException("Diretório não encontrado: " + caminhoDiretorio);

            return Directory.GetFiles(caminhoDiretorio).ToList();
        }

        public void CriarDiretorio(string caminhoDiretorio)
        {
            if (!Directory.Exists(caminhoDiretorio))
                Directory.CreateDirectory(caminhoDiretorio);          
        }

        public string LerArquivo(string caminhoArquivo)
        {
            if (!File.Exists(caminhoArquivo))
                throw new FileNotFoundException("Arquivo não encontrado.", caminhoArquivo);

            return File.ReadAllText(caminhoArquivo);
        }

        public string ObterNomeArquivo(string caminhoArquivo)
        {
            if (!File.Exists(caminhoArquivo))
                throw new FileNotFoundException("Arquivo não encontrado.", caminhoArquivo);

            return Path.GetFileName(caminhoArquivo);
        }

        public string ObterTextoPdf(string caminhoArquivo)
        {
            if (!File.Exists(caminhoArquivo))
                throw new FileNotFoundException("Arquivo não encontrado.", caminhoArquivo);

            var texto = new StringBuilder();

            using (var pdf = PdfDocument.Open(caminhoArquivo))
            {
                foreach (var page in pdf.GetPages())
                    texto.AppendLine(page.Text);
            }

            return texto.ToString();
        }
    }
}