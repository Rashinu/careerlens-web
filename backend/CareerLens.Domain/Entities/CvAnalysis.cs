namespace CareerLens.Domain.Entities;

public class CvAnalysis : BaseEntity
{
    public Guid UserId { get; private set; }
    public string OriginalFileName { get; private set; } = default!;
    public string RawFileUrl { get; private set; } = default!;
    public string? ParsedRawText { get; private set; }
    public string? ParsedData { get; private set; }
    public CvAnalysisStatus Status { get; private set; } = CvAnalysisStatus.Uploaded;

    public User User { get; private set; } = default!;
    public CareerRoadmap? CareerRoadmap { get; private set; }

    private CvAnalysis() { }

    public static CvAnalysis Create(Guid userId, string originalFileName, string rawFileUrl)
    {
        return new CvAnalysis
        {
            UserId = userId,
            OriginalFileName = originalFileName,
            RawFileUrl = rawFileUrl,
            Status = CvAnalysisStatus.Uploaded
        };
    }

    public void SetExtractedText(string rawText)
    {
        ParsedRawText = rawText;
        Status = CvAnalysisStatus.TextExtracted;
        SetUpdatedAt();
    }

    public void SetParsedData(string parsedDataJson)
    {
        ParsedData = parsedDataJson;
        Status = CvAnalysisStatus.Analyzed;
        SetUpdatedAt();
    }

    public void MarkFailed()
    {
        Status = CvAnalysisStatus.Failed;
        SetUpdatedAt();
    }
}

public enum CvAnalysisStatus
{
    Uploaded = 0,
    TextExtracted = 1,
    Analyzed = 2,
    Failed = 3
}
