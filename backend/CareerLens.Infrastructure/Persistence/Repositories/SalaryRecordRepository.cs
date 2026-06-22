using CareerLens.Domain.Entities;
using CareerLens.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareerLens.Infrastructure.Persistence.Repositories;

public class SalaryRecordRepository : GenericRepository<SalaryRecord>, ISalaryRecordRepository
{
    public SalaryRecordRepository(CareerLensDbContext context) : base(context) { }

    public async Task<IReadOnlyList<SalaryRecord>> GetBenchmarkDataAsync(
        string position,
        string? city,
        int yearsOfExperience,
        int toleranceYears = 2,
        CancellationToken ct = default)
    {
        // Not: string.ToLower() karşılaştırması Türkçe büyük İ harfinde .NET/PostgreSQL
        // collation farkı yüzünden yanlış sonuç verir (İstanbul gibi şehirler hiç eşleşmez).
        // EF.Functions.ILike, karşılaştırmayı tamamen veritabanı tarafında ve tutarlı yapar.
        var query = _dbSet.AsNoTracking()
            .Where(s =>
                EF.Functions.ILike(s.Position, $"%{position}%") &&
                s.YearsOfExperience >= yearsOfExperience - toleranceYears &&
                s.YearsOfExperience <= yearsOfExperience + toleranceYears);

        if (!string.IsNullOrWhiteSpace(city))
            query = query.Where(s => EF.Functions.ILike(s.City, city));

        return await query.ToListAsync(ct);
    }
}
