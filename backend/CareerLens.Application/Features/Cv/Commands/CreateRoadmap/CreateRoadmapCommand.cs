using CareerLens.Shared.DTOs.Cv;
using MediatR;

namespace CareerLens.Application.Features.Cv.Commands.CreateRoadmap;

public record CreateRoadmapCommand(Guid CvAnalysisId, Guid UserId, string TargetPosition) : IRequest<RoadmapDto>;
