using CareerLens.Application.Common.Exceptions;
using CareerLens.Domain.Interfaces;
using CareerLens.Shared.DTOs.Cv;
using MediatR;

namespace CareerLens.Application.Features.Cv.Queries.GetCvAnalysis;

public class GetCvAnalysisQueryHandler : IRequestHandler<GetCvAnalysisQuery, CvAnalysisDto>
{
    private readonly IUnitOfWork _uow;

    public GetCvAnalysisQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<CvAnalysisDto> Handle(GetCvAnalysisQuery request, CancellationToken ct)
    {
        var analysis = await _uow.CvAnalyses.GetByIdAsync(request.CvAnalysisId, ct)
            ?? throw new NotFoundException("CvAnalysis", request.CvAnalysisId);

        if (analysis.UserId != request.UserId)
            throw new UnauthorizedException("Bu analiz size ait değil.");

        return new CvAnalysisDto(
            analysis.Id,
            analysis.OriginalFileName,
            analysis.Status.ToString(),
            analysis.ParsedData,
            analysis.CreatedAt);
    }
}
