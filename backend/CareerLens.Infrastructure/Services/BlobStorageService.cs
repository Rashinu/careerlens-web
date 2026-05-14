using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using CareerLens.Application.Common.Interfaces;
using CareerLens.Shared.Constants;
using Microsoft.Extensions.Configuration;

namespace CareerLens.Infrastructure.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(IConfiguration config)
    {
        _blobServiceClient = new BlobServiceClient(config["Azure:BlobStorage:ConnectionString"]);
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct = default)
    {
        var container = _blobServiceClient.GetBlobContainerClient(AppConstants.Cv.BlobContainerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blobName = $"{Guid.NewGuid()}/{fileName}";
        var blobClient = container.GetBlobClient(blobName);

        await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
        return blobName;
    }

    public async Task<Stream> DownloadAsync(string blobName, CancellationToken ct = default)
    {
        var container = _blobServiceClient.GetBlobContainerClient(AppConstants.Cv.BlobContainerName);
        var blobClient = container.GetBlobClient(blobName);
        var response = await blobClient.DownloadStreamingAsync(cancellationToken: ct);
        return response.Value.Content;
    }

    public async Task<string> GetSecureUrlAsync(string blobName, TimeSpan expiry, CancellationToken ct = default)
    {
        var container = _blobServiceClient.GetBlobContainerClient(AppConstants.Cv.BlobContainerName);
        var blobClient = container.GetBlobClient(blobName);

        var sasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.Add(expiry));
        return await Task.FromResult(sasUri.ToString());
    }

    public async Task DeleteAsync(string blobName, CancellationToken ct = default)
    {
        var container = _blobServiceClient.GetBlobContainerClient(AppConstants.Cv.BlobContainerName);
        var blobClient = container.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: ct);
    }
}
