namespace CareerLens.Shared.DTOs.Cv;

public record CvAnalysisDto(
    Guid Id,
    string OriginalFileName,
    string Status,
    string? ParsedData,
    DateTime CreatedAt);

public record CvAnalysisListItemDto(Guid Id, string OriginalFileName, string Status, DateTime CreatedAt);
