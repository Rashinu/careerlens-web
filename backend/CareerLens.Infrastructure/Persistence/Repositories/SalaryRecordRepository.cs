using CareerLens.Domain.Entities;
using CareerLens.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareerLens.Infrastructure.Persistence.Repositories;

public class SalaryRecordRepository : GenericRepository<SalaryRecord>, ISalaryRecordRepository
{
    public SalaryRecordRepository(CareerLensDbContext context) : base(context) { }

    public async Task<IReadOnlyList<SalaryRecord>> GetBenchmarkDataAsync(
        string position,
        string city,
        int yearsOfExperience,
        int toleranceYears = 2,
        CancellationToken ct = default)
    {
        return await _dbSet.AsNoTracking()
            .Where(s =>
                s.Position.ToLower().Contains(position.ToLower()) &&
                s.City.ToLower() == city.ToLower() &&
                s.YearsOfExperience >= yearsOfExperience - toleranceYears &&
                s.YearsOfExperience <= yearsOfExperience + toleranceYears)
            .ToListAsync(ct);
    }
}
