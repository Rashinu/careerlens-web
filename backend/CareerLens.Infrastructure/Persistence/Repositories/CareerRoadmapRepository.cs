using CareerLens.Domain.Entities;
using CareerLens.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareerLens.Infrastructure.Persistence.Repositories;

public class CareerRoadmapRepository : GenericRepository<CareerRoadmap>, ICareerRoadmapRepository
{
    public CareerRoadmapRepository(CareerLensDbContext context) : base(context) { }

    public async Task<CareerRoadmap?> GetByCvAnalysisIdAsync(Guid cvAnalysisId, CancellationToken ct = default) =>
        await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(r => r.CvAnalysisId == cvAnalysisId, ct);

    public async Task<IReadOnlyList<CareerRoadmap>> GetByUserIdAsync(Guid userId, CancellationToken ct = default) =>
        await _dbSet.AsNoTracking()
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.GeneratedAt)
            .ToListAsync(ct);
}
