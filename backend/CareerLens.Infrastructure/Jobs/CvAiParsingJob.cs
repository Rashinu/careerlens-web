using CareerLens.Application.Common.Interfaces;
using CareerLens.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CareerLens.Infrastructure.Jobs;

public class CvAiParsingJob
{
    private readonly IUnitOfWork _uow;
    private readonly ICareerAiService _aiService;
    private readonly ILogger<CvAiParsingJob> _logger;

    public CvAiParsingJob(IUnitOfWork uow, ICareerAiService aiService, ILogger<CvAiParsingJob> logger)
    {
        _uow = uow;
        _aiService = aiService;
        _logger = logger;
    }

    public async Task ExecuteAsync(Guid cvAnalysisId)
    {
        _logger.LogInformation("AI parsing başladı: {Id}", cvAnalysisId);

        var analysis = await _uow.CvAnalyses.GetByIdAsync(cvAnalysisId);
        if (analysis is null)
        {
            _logger.LogWarning("CvAnalysis bulunamadı: {Id}", cvAnalysisId);
            return;
        }

        if (string.IsNullOrWhiteSpace(analysis.ParsedRawText))
        {
            _logger.LogWarning("Ham metin yok, AI parsing atlanıyor: {Id}", cvAnalysisId);
            return;
        }

        try
        {
            var parsedJson = await _aiService.ParseCvAsync(analysis.ParsedRawText);

            analysis.SetParsedData(parsedJson);
            _uow.CvAnalyses.Update(analysis);
            await _uow.SaveChangesAsync();

            _logger.LogInformation("AI parsing tamamlandı: {Id}", cvAnalysisId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI parsing başarısız: {Id}", cvAnalysisId);
            analysis.MarkFailed();
            _uow.CvAnalyses.Update(analysis);
            await _uow.SaveChangesAsync();
            throw;
        }
    }
}
