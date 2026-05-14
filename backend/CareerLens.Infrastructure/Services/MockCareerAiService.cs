using CareerLens.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CareerLens.Infrastructure.Services;

public class MockCareerAiService : ICareerAiService
{
    private readonly ILogger<MockCareerAiService> _logger;

    public MockCareerAiService(ILogger<MockCareerAiService> logger) => _logger = logger;

    public Task<string> ParseCvAsync(string rawText, CancellationToken ct = default)
    {
        _logger.LogInformation("[MOCK] CV parse çalışıyor — gerçek OpenAI çağrısı yok");

        var result = new
        {
            fullName = "Test Kullanıcı",
            email = "test@example.com",
            phone = "+90 555 000 0000",
            currentPosition = "Software Developer",
            yearsOfExperience = 3,
            techStack = new[] { ".NET", "C#", "PostgreSQL", "Docker", "React" },
            education = new[] { new { degree = "Lisans", school = "XYZ Üniversitesi", year = 2021 } },
            languages = new[] { "Türkçe", "İngilizce" },
            summary = "Deneyimli backend geliştirici (mock veri)"
        };

        return Task.FromResult(JsonSerializer.Serialize(result));
    }

    public Task<string> GenerateRoadmapAsync(string parsedCvJson, string targetPosition, string salaryBenchmarkJson, CancellationToken ct = default)
    {
        _logger.LogInformation("[MOCK] Roadmap üretiliyor — hedef: {Target}", targetPosition);

        var result = new
        {
            currentScore = 65,
            gapAnalysis = new
            {
                strengths = new[] { ".NET uzmanlığı", "PostgreSQL deneyimi", "Docker bilgisi" },
                gaps = new[] { "Microservices mimarisi", "Kubernetes", "AWS/Azure sertifikası" },
                estimatedSalaryRange = new { min = 55000, max = 75000 }
            },
            recommendations = new
            {
                immediate = new[] { "GitHub portföyünü güncelleyin", "LinkedIn profilinizi optimize edin" },
                shortTerm = new[] { "Kubernetes eğitimi alın", "AWS Solutions Architect sınavına hazırlanın" },
                longTerm = new[] { "Microservices projesi geliştirin", "Teknik blog yazın" },
                courses = new[]
                {
                    new { name = "Kubernetes ile Container Orkestrasyonu", platform = "Udemy", priority = "high" },
                    new { name = "AWS Solutions Architect", platform = "A Cloud Guru", priority = "high" },
                    new { name = "Clean Architecture ile .NET", platform = "Pluralsight", priority = "medium" }
                }
            }
        };

        return Task.FromResult(JsonSerializer.Serialize(result));
    }
}
