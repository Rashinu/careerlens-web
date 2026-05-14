using CareerLens.Application.Common.Exceptions;
using CareerLens.Application.Common.Interfaces;
using CareerLens.Domain.Entities;
using CareerLens.Domain.Interfaces;
using CareerLens.Shared.DTOs.Cv;
using MediatR;

namespace CareerLens.Application.Features.Cv.Commands.UploadCv;

public class UploadCvCommandHandler : IRequestHandler<UploadCvCommand, CvAnalysisListItemDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IBlobStorageService _blob;
    private readonly ICvProcessingJobService _jobService;

    public UploadCvCommandHandler(IUnitOfWork uow, IBlobStorageService blob, ICvProcessingJobService jobService)
    {
        _uow = uow;
        _blob = blob;
        _jobService = jobService;
    }

    public async Task<CvAnalysisListItemDto> Handle(UploadCvCommand request, CancellationToken ct)
    {
        var user = await _uow.Users.GetByIdAsync(request.UserId, ct)
            ?? throw new NotFoundException("User", request.UserId);

        var blobName = await _blob.UploadAsync(request.FileStream, request.FileName, request.ContentType, ct);

        var analysis = CvAnalysis.Create(request.UserId, request.FileName, blobName);
        await _uow.CvAnalyses.AddAsync(analysis, ct);
        await _uow.SaveChangesAsync(ct);

        // Arka planda text extraction başlat
        _jobService.EnqueueTextExtraction(analysis.Id);

        return new CvAnalysisListItemDto(analysis.Id, analysis.OriginalFileName, analysis.Status.ToString(), analysis.CreatedAt);
    }
}
