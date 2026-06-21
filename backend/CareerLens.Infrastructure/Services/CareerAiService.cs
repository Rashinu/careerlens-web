using CareerLens.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace CareerLens.Infrastructure.Services;

public class CareerAiService : ICareerAiService
{
    private readonly Kernel _kernel;
    private readonly ILogger<CareerAiService> _logger;

    private const string CvParsePrompt = """
        Aşağıdaki CV metnini analiz et ve SADECE aşağıdaki JSON formatında döndür. Başka hiçbir metin ekleme.
        Metindeki kişisel bilgileri olduğu gibi kullan. CV metninden çıkaramadığın alanlar için null kullan.

        {
          "fullName": "string veya null",
          "email": "string veya null",
          "phone": "string veya null",
          "currentPosition": "string veya null",
          "yearsOfExperience": number veya null,
          "techStack": ["teknoloji1", "teknoloji2"],
          "education": [{"degree": "string", "school": "string", "year": number veya null}],
          "languages": ["dil1", "dil2"],
          "summary": "kısa özet veya null"
        }

        CV Metni:
        {{$cvText}}
        """;

    private const string RoadmapPrompt = """
        Kariyer koçu olarak aşağıdaki profili analiz et ve SADECE JSON formatında döndür.

        Kullanıcı Profili (JSON):
        {{$profile}}

        Hedef Pozisyon: {{$targetPosition}}

        Maaş Benchmark (TÜFE ile enflasyon düzeltmeli, ülke çapında):
        {{$benchmark}}

        ÖNEMLİ: estimatedSalaryRange ve her önerideki estimatedMonthlyImpact alanlarını YALNIZCA
        yukarıdaki benchmark'taki p25Salary/p50Salary/p75Salary değerlerine dayanarak tahmin et.
        sampleCount düşükse (<5) veya benchmark verisi yoksa, bu alanlar için null kullan — tahmin icat etme.

        Şu formatta JSON döndür:
        {
          "currentScore": 0-100 arası sayı,
          "gapAnalysis": {
            "strengths": ["güçlü yön 1", "güçlü yön 2"],
            "gaps": ["eksik beceri 1", "eksik beceri 2"],
            "estimatedSalaryRange": {"min": sayı veya null, "max": sayı veya null}
          },
          "recommendations": {
            "immediate": [{"title": "...", "description": "...", "estimatedMonthlyImpact": sayı veya null}],
            "shortTerm": [{"title": "...", "description": "...", "estimatedMonthlyImpact": sayı veya null}],
            "longTerm": [{"title": "...", "description": "...", "estimatedMonthlyImpact": sayı veya null}],
            "courses": [{"name": "kurs adı", "platform": "platform", "priority": "high/medium/low"}]
          }
        }
        """;

    public CareerAiService(IConfiguration config, ILogger<CareerAiService> logger)
    {
        _logger = logger;
        var builder = Kernel.CreateBuilder();
        builder.AddOpenAIChatCompletion(
            modelId: config["OpenAI:Model"] ?? "gpt-3.5-turbo",
            apiKey: config["OpenAI:ApiKey"]!);
        _kernel = builder.Build();
    }

    public async Task<string> ParseCvAsync(string rawText, CancellationToken ct = default)
    {
        var func = _kernel.CreateFunctionFromPrompt(CvParsePrompt, new OpenAIPromptExecutionSettings
        {
            ResponseFormat = "json_object",
            MaxTokens = 1000,
            Temperature = 0.1
        });

        var result = await _kernel.InvokeAsync(func, new KernelArguments { ["cvText"] = SanitizeInput(rawText) }, ct);
        _logger.LogInformation("CV parse tamamlandı. Token kullanımı loglandı.");
        return result.ToString();
    }

    public async Task<string> GenerateRoadmapAsync(string parsedCvJson, string targetPosition, string salaryBenchmarkJson, CancellationToken ct = default)
    {
        var func = _kernel.CreateFunctionFromPrompt(RoadmapPrompt, new OpenAIPromptExecutionSettings
        {
            ResponseFormat = "json_object",
            MaxTokens = 2000,
            Temperature = 0.3
        });

        var args = new KernelArguments
        {
            ["profile"] = parsedCvJson,
            ["targetPosition"] = SanitizeInput(targetPosition),
            ["benchmark"] = salaryBenchmarkJson
        };

        var result = await _kernel.InvokeAsync(func, args, ct);
        _logger.LogInformation("Roadmap üretildi. Hedef: {Target}", targetPosition);
        return result.ToString();
    }

    private static string SanitizeInput(string input)
    {
        // Prompt injection koruması: özel direktif kalıplarını temizle
        var dangerous = new[] { "ignore previous", "system:", "assistant:", "###", "```" };
        foreach (var d in dangerous)
            input = input.Replace(d, string.Empty, StringComparison.OrdinalIgnoreCase);
        return input.Length > 8000 ? input[..8000] : input;
    }
}
