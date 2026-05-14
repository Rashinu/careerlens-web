using CareerLens.Domain.Interfaces;
using CareerLens.Shared.DTOs.Cv;
using MediatR;

namespace CareerLens.Application.Features.Cv.Queries.GetCvList;

public class GetCvListQueryHandler : IRequestHandler<GetCvListQuery, IReadOnlyList<CvAnalysisListItemDto>>
{
    private readonly IUnitOfWork _uow;

    public GetCvListQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IReadOnlyList<CvAnalysisListItemDto>> Handle(GetCvListQuery request, CancellationToken ct)
    {
        var analyses = await _uow.CvAnalyses.GetByUserIdAsync(request.UserId, ct);
        return analyses
            .Select(a => new CvAnalysisListItemDto(a.Id, a.OriginalFileName, a.Status.ToString(), a.CreatedAt))
            .ToList();
    }
}
