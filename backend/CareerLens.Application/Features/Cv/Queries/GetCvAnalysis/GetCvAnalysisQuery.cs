using CareerLens.Shared.DTOs.Cv;
using MediatR;

namespace CareerLens.Application.Features.Cv.Queries.GetCvAnalysis;

public record GetCvAnalysisQuery(Guid CvAnalysisId, Guid UserId) : IRequest<CvAnalysisDto>;
