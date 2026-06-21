using CareerLens.Application.Common.Exceptions;
using CareerLens.Application.Common.Interfaces;
using CareerLens.Application.Features.Salary;
using CareerLens.Domain.Entities;
using CareerLens.Domain.Interfaces;
using CareerLens.Shared.DTOs.Cv;
using MediatR;
using System.Text.Json;

namespace CareerLens.Application.Features.Cv.Commands.CreateRoadmap;

public class CreateRoadmapCommandHandler : IRequestHandler<CreateRoadmapCommand, RoadmapDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICareerAiService _aiService;
    private readonly ICpiIndexService _cpiIndex;

    public CreateRoadmapCommandHandler(IUnitOfWork uow, ICareerAiService aiService, ICpiIndexService cpiIndex)
    {
        _uow = uow;
        _aiService = aiService;
        _cpiIndex = cpiIndex;
    }

    public async Task<RoadmapDto> Handle(CreateRoadmapCommand request, CancellationToken ct)
    {
        var analysis = await _uow.CvAnalyses.GetByIdAsync(request.CvAnalysisId, ct)
            ?? throw new NotFoundException("CvAnalysis", request.CvAnalysisId);

        if (analysis.UserId != request.UserId)
            throw new UnauthorizedException("Bu analiz size ait değil.");

        if (analysis.Status != CvAnalysisStatus.Analyzed || analysis.ParsedData is null)
            throw new InvalidOperationException("CV analizi henüz tamamlanmadı. Lütfen bekleyin.");

        var yearsOfExperience = ExtractYearsOfExperience(analysis.ParsedData);

        // Şehir bilgisi CV'den çıkarılamadığı için ülke çapında (city: null) benchmark alınır.
        var benchmarkRecords = await _uow.SalaryRecords.GetBenchmarkDataAsync(
            request.TargetPosition, city: null, yearsOfExperience, ct: ct);

        var benchmark = await SalaryBenchmarkCalculator.CalculateAsync(benchmarkRecords, _cpiIndex, ct);

        var benchmarkJson = JsonSerializer.Serialize(new
        {
            sampleCount = benchmarkRecords.Count,
            p25Salary = benchmark.P25,
            p50Salary = benchmark.P50,
            p75Salary = benchmark.P75,
            isInflationAdjusted = benchmark.IsInflationAdjusted,
            position = request.TargetPosition
        });

        var roadmapJson = await _aiService.GenerateRoadmapAsync(
            analysis.ParsedData, request.TargetPosition, benchmarkJson, ct);

        int score = 50;
        string gapAnalysisJson = "{}";
        string recommendationsJson = "{}";

        try
        {
            var root = JsonSerializer.Deserialize<JsonElement>(roadmapJson);
            if (root.TryGetProperty("currentScore", out var scoreEl))
                score = scoreEl.GetInt32();
            if (root.TryGetProperty("gapAnalysis", out var gapEl))
                gapAnalysisJson = gapEl.GetRawText();
            if (root.TryGetProperty("recommendations", out var recEl))
                recommendationsJson = recEl.GetRawText();
        }
        catch { }

        var roadmap = CareerRoadmap.Create(
            request.UserId,
            request.CvAnalysisId,
            request.TargetPosition,
            score,
            gapAnalysisJson,
            recommendationsJson);

        await _uow.CareerRoadmaps.AddAsync(roadmap, ct);
        await _uow.SaveChangesAsync(ct);

        return new RoadmapDto(
            roadmap.Id,
            roadmap.CvAnalysisId,
            roadmap.TargetPosition,
            roadmap.CurrentScore,
            roadmap.GapAnalysis,
            roadmap.Recommendations,
            roadmap.GeneratedAt);
    }

    private static int ExtractYearsOfExperience(string parsedDataJson)
    {
        try
        {
            var root = JsonSerializer.Deserialize<JsonElement>(parsedDataJson);
            if (root.TryGetProperty("yearsOfExperience", out var el) && el.ValueKind == JsonValueKind.Number)
                return el.GetInt32();
        }
        catch { }
        return 3; // varsayılan: orta seviye deneyim
    }
}
