namespace CareerLens.Domain.Entities;

public class CareerRoadmap : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid CvAnalysisId { get; private set; }
    public int CurrentScore { get; private set; }
    public string TargetPosition { get; private set; } = default!;
    public string GapAnalysis { get; private set; } = default!;
    public string Recommendations { get; private set; } = default!;
    public DateTime GeneratedAt { get; private set; } = DateTime.UtcNow;

    public User User { get; private set; } = default!;
    public CvAnalysis CvAnalysis { get; private set; } = default!;

    private CareerRoadmap() { }

    public static CareerRoadmap Create(
        Guid userId,
        Guid cvAnalysisId,
        string targetPosition,
        int currentScore,
        string gapAnalysisJson,
        string recommendationsJson)
    {
        return new CareerRoadmap
        {
            UserId = userId,
            CvAnalysisId = cvAnalysisId,
            TargetPosition = targetPosition,
            CurrentScore = currentScore,
            GapAnalysis = gapAnalysisJson,
            Recommendations = recommendationsJson,
            GeneratedAt = DateTime.UtcNow
        };
    }
}
