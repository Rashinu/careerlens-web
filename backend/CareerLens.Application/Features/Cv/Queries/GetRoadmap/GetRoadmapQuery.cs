using CareerLens.Shared.DTOs.Cv;
using MediatR;

namespace CareerLens.Application.Features.Cv.Queries.GetRoadmap;

public record GetRoadmapQuery(Guid CvAnalysisId, Guid UserId) : IRequest<RoadmapDto>;
