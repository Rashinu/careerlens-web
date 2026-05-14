using CareerLens.Domain.Interfaces;
using CareerLens.Infrastructure.Persistence.Repositories;

namespace CareerLens.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly CareerLensDbContext _context;
    private bool _disposed;

    public IUserRepository Users { get; }
    public ICvAnalysisRepository CvAnalyses { get; }
    public ISalaryRecordRepository SalaryRecords { get; }
    public ICareerRoadmapRepository CareerRoadmaps { get; }

    public UnitOfWork(CareerLensDbContext context)
    {
        _context = context;
        Users = new UserRepository(context);
        CvAnalyses = new CvAnalysisRepository(context);
        SalaryRecords = new SalaryRecordRepository(context);
        CareerRoadmaps = new CareerRoadmapRepository(context);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        await _context.SaveChangesAsync(ct);

    public void Dispose()
    {
        if (!_disposed)
        {
            _context.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}
