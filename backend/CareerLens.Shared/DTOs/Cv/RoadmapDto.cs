namespace CareerLens.Shared.DTOs.Cv;

public record RoadmapDto(
    Guid Id,
    Guid CvAnalysisId,
    string TargetPosition,
    int CurrentScore,
    string GapAnalysis,
    string Recommendations,
    DateTime GeneratedAt);
