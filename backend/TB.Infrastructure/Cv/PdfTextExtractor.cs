using System.Text;
using TB.Application.Abstractions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace TB.Infrastructure.Cv;

public sealed class PdfTextExtractor : IPdfTextExtractor
{
    public string ExtractText(Stream pdfStream)
    {
        if (pdfStream.CanSeek)
        {
            pdfStream.Position = 0;
        }

        using var document = PdfDocument.Open(pdfStream);
        var builder = new StringBuilder();

        foreach (Page page in document.GetPages())
        {
            builder.AppendLine(page.Text);
        }

        return builder.ToString();
    }
}
