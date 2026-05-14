namespace CareerLens.Application.Common.Interfaces;

public interface ICareerAiService
{
    Task<string> ParseCvAsync(string rawText, CancellationToken ct = default);
    Task<string> GenerateRoadmapAsync(string parsedCvJson, string targetPosition, string salaryBenchmarkJson, CancellationToken ct = default);
}
