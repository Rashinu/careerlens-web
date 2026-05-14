using CareerLens.Application.Common.Interfaces;
using CareerLens.Domain.Interfaces;
using CareerLens.Shared.Constants;
using CareerLens.Shared.DTOs.Salary;
using MediatR;

namespace CareerLens.Application.Features.Salary.Queries.GetBenchmark;

public class GetSalaryBenchmarkQueryHandler : IRequestHandler<GetSalaryBenchmarkQuery, SalaryBenchmarkDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICacheService _cache;

    public GetSalaryBenchmarkQueryHandler(IUnitOfWork uow, ICacheService cache)
    {
        _uow = uow;
        _cache = cache;
    }

    public async Task<SalaryBenchmarkDto> Handle(GetSalaryBenchmarkQuery request, CancellationToken ct)
    {
        var cacheKey = $"{AppConstants.Cache.SalaryBenchmarkKeyPrefix}{request.Position}:{request.City}:{request.YearsOfExperience}";
        var cached = await _cache.GetAsync<SalaryBenchmarkDto>(cacheKey, ct);
        if (cached is not null) return cached;

        var records = await _uow.SalaryRecords.GetBenchmarkDataAsync(
            request.Position, request.City, request.YearsOfExperience, ct: ct);

        if (records.Count == 0)
            return new SalaryBenchmarkDto(0, 0, 0, 0, request.Position, request.City, request.YearsOfExperience);

        var sorted = records.Select(r => r.NetSalary).OrderBy(s => s).ToList();
        var p25 = Percentile(sorted, 25);
        var p50 = Percentile(sorted, 50);
        var p75 = Percentile(sorted, 75);

        var result = new SalaryBenchmarkDto(p25, p50, p75, records.Count, request.Position, request.City, request.YearsOfExperience);
        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(AppConstants.Cache.SalaryBenchmarkCacheMinutes), ct);

        return result;
    }

    private static decimal Percentile(List<decimal> sortedData, int percentile)
    {
        if (sortedData.Count == 0) return 0;
        var index = (int)Math.Ceiling(percentile / 100.0 * sortedData.Count) - 1;
        return sortedData[Math.Max(0, index)];
    }
}
