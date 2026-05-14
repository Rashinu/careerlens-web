namespace CareerLens.Shared.DTOs.Dashboard;

public record DashboardDto(
    int TotalCvAnalyses,
    int CompletedAnalyses,
    string? LatestCvStatus,
    int? LatestRoadmapScore,
    string? LatestTargetPosition);
