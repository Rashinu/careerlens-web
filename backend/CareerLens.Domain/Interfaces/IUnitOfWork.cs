namespace CareerLens.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    ICvAnalysisRepository CvAnalyses { get; }
    ISalaryRecordRepository SalaryRecords { get; }
    ICareerRoadmapRepository CareerRoadmaps { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
