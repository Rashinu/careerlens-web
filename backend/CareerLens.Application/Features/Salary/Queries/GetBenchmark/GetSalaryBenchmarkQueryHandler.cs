using CareerLens.Application.Common.Interfaces;
using CareerLens.Application.Features.Salary;
using CareerLens.Domain.Interfaces;
using CareerLens.Shared.Constants;
using CareerLens.Shared.DTOs.Salary;
using MediatR;

namespace CareerLens.Application.Features.Salary.Queries.GetBenchmark;

public class GetSalaryBenchmarkQueryHandler : IRequestHandler<GetSalaryBenchmarkQuery, SalaryBenchmarkDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICacheService _cache;
    private readonly ICpiIndexService _cpiIndex;

    public GetSalaryBenchmarkQueryHandler(IUnitOfWork uow, ICacheService cache, ICpiIndexService cpiIndex)
    {
        _uow = uow;
        _cache = cache;
        _cpiIndex = cpiIndex;
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

        var benchmark = await SalaryBenchmarkCalculator.CalculateAsync(records, _cpiIndex, ct);

        var result = new SalaryBenchmarkDto(
            benchmark.P25, benchmark.P50, benchmark.P75, records.Count,
            request.Position, request.City, request.YearsOfExperience, benchmark.IsInflationAdjusted);
        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(AppConstants.Cache.SalaryBenchmarkCacheMinutes), ct);

        return result;
    }
}
