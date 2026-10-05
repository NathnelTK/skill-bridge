namespace TB.Application.Abstractions;

public interface IPdfTextExtractor
{
    /// <summary>
    /// Extracts the plain text of a PDF document. Returns an empty string when the
    /// document contains no extractable text.
    /// </summary>
    string ExtractText(Stream pdfStream);
}
