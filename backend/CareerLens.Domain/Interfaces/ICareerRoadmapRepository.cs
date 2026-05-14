using CareerLens.Domain.Entities;

namespace CareerLens.Domain.Interfaces;

public interface ICareerRoadmapRepository : IRepository<CareerRoadmap>
{
    Task<CareerRoadmap?> GetByCvAnalysisIdAsync(Guid cvAnalysisId, CancellationToken ct = default);
    Task<IReadOnlyList<CareerRoadmap>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
}
