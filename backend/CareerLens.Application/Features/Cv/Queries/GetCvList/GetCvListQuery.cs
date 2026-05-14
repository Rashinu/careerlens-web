using CareerLens.Shared.DTOs.Cv;
using MediatR;

namespace CareerLens.Application.Features.Cv.Queries.GetCvList;

public record GetCvListQuery(Guid UserId) : IRequest<IReadOnlyList<CvAnalysisListItemDto>>;
