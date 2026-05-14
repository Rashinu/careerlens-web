namespace CareerLens.Application.Common.Interfaces;

public interface ICvProcessingJobService
{
    void EnqueueTextExtraction(Guid cvAnalysisId);
    void EnqueueAiParsing(Guid cvAnalysisId);
}
