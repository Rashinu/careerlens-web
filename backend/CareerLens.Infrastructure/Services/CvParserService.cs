using CareerLens.Application.Common.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using UglyToad.PdfPig;
using System.Text;

namespace CareerLens.Infrastructure.Services;

public class CvParserService : ICvParserService
{
    public async Task<string> ExtractTextAsync(Stream fileStream, string fileName, CancellationToken ct = default)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();

        return ext switch
        {
            ".pdf" => await ExtractFromPdfAsync(fileStream),
            ".docx" => await ExtractFromDocxAsync(fileStream),
            _ => throw new NotSupportedException($"Desteklenmeyen dosya formatı: {ext}")
        };
    }

    private static Task<string> ExtractFromPdfAsync(Stream stream)
    {
        var sb = new StringBuilder();
        using var document = PdfDocument.Open(stream);
        foreach (var page in document.GetPages())
            sb.AppendLine(page.Text);

        return Task.FromResult(sb.ToString());
    }

    private static Task<string> ExtractFromDocxAsync(Stream stream)
    {
        var sb = new StringBuilder();
        using var doc = WordprocessingDocument.Open(stream, false);
        var body = doc.MainDocumentPart?.Document?.Body;
        if (body is null) return Task.FromResult(string.Empty);

        foreach (var para in body.Elements<Paragraph>())
            sb.AppendLine(para.InnerText);

        return Task.FromResult(sb.ToString());
    }
}
