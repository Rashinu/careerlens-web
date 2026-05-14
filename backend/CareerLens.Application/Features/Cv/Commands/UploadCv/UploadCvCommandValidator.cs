using CareerLens.Shared.Constants;
using FluentValidation;

namespace CareerLens.Application.Features.Cv.Commands.UploadCv;

public class UploadCvCommandValidator : AbstractValidator<UploadCvCommand>
{
    public UploadCvCommandValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .Must(name => AppConstants.Cv.AllowedExtensions.Contains(Path.GetExtension(name).ToLowerInvariant()))
            .WithMessage("Sadece PDF ve DOCX dosyaları kabul edilir.");

        RuleFor(x => x.FileSizeBytes)
            .LessThanOrEqualTo(AppConstants.Cv.MaxFileSizeBytes)
            .WithMessage("Dosya boyutu 5MB'dan büyük olamaz.");

        RuleFor(x => x.ContentType)
            .Must(ct => AppConstants.Cv.AllowedMimeTypes.Contains(ct))
            .WithMessage("Geçersiz dosya türü.");
    }
}
