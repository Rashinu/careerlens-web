namespace CareerLens.Application.Common.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct = default);
    Task<Stream> DownloadAsync(string blobName, CancellationToken ct = default);
    Task<string> GetSecureUrlAsync(string blobName, TimeSpan expiry, CancellationToken ct = default);
    Task DeleteAsync(string blobName, CancellationToken ct = default);
}
