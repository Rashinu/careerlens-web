namespace CareerLens.Application.Common.Interfaces;

public interface ICvParserService
{
    Task<string> ExtractTextAsync(Stream fileStream, string fileName, CancellationToken ct = default);
}
