using CareerLens.Application.Common.Interfaces;
using CareerLens.Domain.Interfaces;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace CareerLens.Infrastructure.Jobs;

public class CvTextExtractionJob
{
    private readonly IUnitOfWork _uow;
    private readonly IBlobStorageService _blob;
    private readonly ICvParserService _parser;
    private readonly IBackgroundJobClient _jobClient;
    private readonly ILogger<CvTextExtractionJob> _logger;

    public CvTextExtractionJob(
        IUnitOfWork uow,
        IBlobStorageService blob,
        ICvParserService parser,
        IBackgroundJobClient jobClient,
        ILogger<CvTextExtractionJob> logger)
    {
        _uow = uow;
        _blob = blob;
        _parser = parser;
        _jobClient = jobClient;
        _logger = logger;
    }

    public async Task ExecuteAsync(Guid cvAnalysisId)
    {
        _logger.LogInformation("Text extraction başladı: {Id}", cvAnalysisId);

        var analysis = await _uow.CvAnalyses.GetByIdAsync(cvAnalysisId);
        if (analysis is null)
        {
            _logger.LogWarning("CvAnalysis bulunamadı: {Id}", cvAnalysisId);
            return;
        }

        try
        {
            using var stream = await _blob.DownloadAsync(analysis.RawFileUrl);
            var rawText = await _parser.ExtractTextAsync(stream, analysis.OriginalFileName);

            analysis.SetExtractedText(rawText);
            _uow.CvAnalyses.Update(analysis);
            await _uow.SaveChangesAsync();

            _logger.LogInformation("Text extraction tamamlandı: {Id}", cvAnalysisId);

            // AI parsing job'unu sıraya ekle
            _jobClient.Enqueue<CvAiParsingJob>(j => j.ExecuteAsync(cvAnalysisId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Text extraction başarısız: {Id}", cvAnalysisId);
            analysis.MarkFailed();
            _uow.CvAnalyses.Update(analysis);
            await _uow.SaveChangesAsync();
            throw;
        }
    }
}
