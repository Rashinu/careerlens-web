using CareerLens.Domain.Interfaces;
using CareerLens.Shared.DTOs.Dashboard;
using MediatR;

namespace CareerLens.Application.Features.Dashboard.Queries.GetDashboard;

public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardDto>
{
    private readonly IUnitOfWork _uow;

    public GetDashboardQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<DashboardDto> Handle(GetDashboardQuery request, CancellationToken ct)
    {
        var analyses = await _uow.CvAnalyses.GetByUserIdAsync(request.UserId, ct);
        var roadmaps = await _uow.CareerRoadmaps.GetByUserIdAsync(request.UserId, ct);

        var latestAnalysis = analyses.FirstOrDefault();
        var latestRoadmap = roadmaps.FirstOrDefault();

        return new DashboardDto(
            TotalCvAnalyses: analyses.Count,
            CompletedAnalyses: analyses.Count(a => a.Status == Domain.Entities.CvAnalysisStatus.Analyzed),
            LatestCvStatus: latestAnalysis?.Status.ToString(),
            LatestRoadmapScore: latestRoadmap?.CurrentScore,
            LatestTargetPosition: latestRoadmap?.TargetPosition);
    }
}
