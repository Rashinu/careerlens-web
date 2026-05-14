using CareerLens.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CareerLens.Infrastructure.Services;

public class LocalBlobStorageService : IBlobStorageService
{
    private readonly string _basePath;
    private readonly ILogger<LocalBlobStorageService> _logger;

    public LocalBlobStorageService(IConfiguration config, ILogger<LocalBlobStorageService> logger)
    {
        _logger = logger;
        _basePath = config["LocalStorage:BasePath"]
            ?? Path.Combine(Directory.GetCurrentDirectory(), "local-storage", "cv-files");
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct = default)
    {
        var blobName = $"{Guid.NewGuid()}/{fileName}";
        var fullPath = Path.Combine(_basePath, blobName.Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        await using var fileOut = File.Create(fullPath);
        await fileStream.CopyToAsync(fileOut, ct);

        _logger.LogInformation("Dosya lokal olarak kaydedildi: {Path}", fullPath);
        return blobName;
    }

    public Task<Stream> DownloadAsync(string blobName, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(_basePath, blobName.Replace('/', Path.DirectorySeparatorChar));
        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Lokal blob bulunamadı: {blobName}");

        Stream stream = File.OpenRead(fullPath);
        return Task.FromResult(stream);
    }

    public Task<string> GetSecureUrlAsync(string blobName, TimeSpan expiry, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(_basePath, blobName.Replace('/', Path.DirectorySeparatorChar));
        return Task.FromResult($"file://{fullPath}");
    }

    public Task DeleteAsync(string blobName, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(_basePath, blobName.Replace('/', Path.DirectorySeparatorChar));
        if (File.Exists(fullPath)) File.Delete(fullPath);
        return Task.CompletedTask;
    }
}
