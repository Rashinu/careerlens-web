using CareerLens.Shared.DTOs.Cv;
using MediatR;

namespace CareerLens.Application.Features.Cv.Commands.UploadCv;

public record UploadCvCommand(
    Guid UserId,
    string FileName,
    Stream FileStream,
    string ContentType,
    long FileSizeBytes) : IRequest<CvAnalysisListItemDto>;
