using Herval.RPA.Sdk.Interfaces;
using UglyToad.PdfPig;

namespace Herval.RPA.Sdk.Services
{
    public class PdfService : IPdfService
    {
        public bool PdfContemTexto(string pdfPath, string textoProcurado)
        {
            using var document = PdfDocument.Open(pdfPath);

            foreach (var page in document.GetPages())
            {
                if (page.Text.Contains(textoProcurado, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}