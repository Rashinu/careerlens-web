using CareerLens.Application.Common.Exceptions;
using CareerLens.Domain.Interfaces;
using CareerLens.Shared.DTOs.Cv;
using MediatR;

namespace CareerLens.Application.Features.Cv.Queries.GetRoadmap;

public class GetRoadmapQueryHandler : IRequestHandler<GetRoadmapQuery, RoadmapDto>
{
    private readonly IUnitOfWork _uow;

    public GetRoadmapQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<RoadmapDto> Handle(GetRoadmapQuery request, CancellationToken ct)
    {
        var roadmap = await _uow.CareerRoadmaps.GetByCvAnalysisIdAsync(request.CvAnalysisId, ct)
            ?? throw new NotFoundException("CareerRoadmap", request.CvAnalysisId);

        if (roadmap.UserId != request.UserId)
            throw new UnauthorizedException("Bu roadmap size ait değil.");

        return new RoadmapDto(
            roadmap.Id,
            roadmap.CvAnalysisId,
            roadmap.TargetPosition,
            roadmap.CurrentScore,
            roadmap.GapAnalysis,
            roadmap.Recommendations,
            roadmap.GeneratedAt);
    }
}
