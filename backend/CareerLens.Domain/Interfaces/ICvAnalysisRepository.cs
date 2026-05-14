using CareerLens.Domain.Entities;

namespace CareerLens.Domain.Interfaces;

public interface ICvAnalysisRepository : IRepository<CvAnalysis>
{
    Task<IReadOnlyList<CvAnalysis>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<CvAnalysis?> GetByIdWithUserAsync(Guid id, CancellationToken ct = default);
}
