using CareerLens.Domain.Entities;

namespace CareerLens.Domain.Interfaces;

public interface ISalaryRecordRepository : IRepository<SalaryRecord>
{
    /// <param name="city">Null verilirse tüm şehirler dahil edilir (ülke çapında benchmark).</param>
    Task<IReadOnlyList<SalaryRecord>> GetBenchmarkDataAsync(
        string position,
        string? city,
        int yearsOfExperience,
        int toleranceYears = 2,
        CancellationToken ct = default);
}
