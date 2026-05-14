using CareerLens.Domain.Entities;
using CareerLens.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareerLens.Infrastructure.Persistence.Repositories;

public class CvAnalysisRepository : GenericRepository<CvAnalysis>, ICvAnalysisRepository
{
    public CvAnalysisRepository(CareerLensDbContext context) : base(context) { }

    public async Task<IReadOnlyList<CvAnalysis>> GetByUserIdAsync(Guid userId, CancellationToken ct = default) =>
        await _dbSet.AsNoTracking()
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(ct);

    public async Task<CvAnalysis?> GetByIdWithUserAsync(Guid id, CancellationToken ct = default) =>
        await _dbSet.Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id, ct);
}
