namespace CareerLens.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public UserPlan Plan { get; private set; } = UserPlan.Free;
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiresAt { get; private set; }

    public ICollection<CvAnalysis> CvAnalyses { get; private set; } = new List<CvAnalysis>();
    public ICollection<CareerRoadmap> CareerRoadmaps { get; private set; } = new List<CareerRoadmap>();

    private User() { }

    public static User Create(string email, string passwordHash)
    {
        return new User
        {
            Email = email.ToLowerInvariant(),
            PasswordHash = passwordHash
        };
    }

    public void UpdateProfile(string? firstName, string? lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        SetUpdatedAt();
    }

    public void SetRefreshToken(string token, DateTime expiresAt)
    {
        RefreshToken = token;
        RefreshTokenExpiresAt = expiresAt;
        SetUpdatedAt();
    }

    public void RevokeRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiresAt = null;
        SetUpdatedAt();
    }

    public void UpgradePlan(UserPlan plan)
    {
        Plan = plan;
        SetUpdatedAt();
    }
}

public enum UserPlan
{
    Free = 0,
    Pro = 1,
    HR = 2
}
