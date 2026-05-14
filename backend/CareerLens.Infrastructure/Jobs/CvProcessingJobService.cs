using CareerLens.Application.Common.Interfaces;
using Hangfire;

namespace CareerLens.Infrastructure.Jobs;

public class CvProcessingJobService : ICvProcessingJobService
{
    private readonly IBackgroundJobClient _jobClient;

    public CvProcessingJobService(IBackgroundJobClient jobClient) => _jobClient = jobClient;

    public void EnqueueTextExtraction(Guid cvAnalysisId) =>
        _jobClient.Enqueue<CvTextExtractionJob>(j => j.ExecuteAsync(cvAnalysisId));

    public void EnqueueAiParsing(Guid cvAnalysisId) =>
        _jobClient.Enqueue<CvAiParsingJob>(j => j.ExecuteAsync(cvAnalysisId));
}
