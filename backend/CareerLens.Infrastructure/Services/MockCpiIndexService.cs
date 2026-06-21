using CareerLens.Application.Common.Interfaces;

namespace CareerLens.Infrastructure.Services;

public class MockCpiIndexService : ICpiIndexService
{
    public Task<decimal> GetAdjustmentFactorAsync(DateTime fromDate, CancellationToken ct = default)
        => Task.FromResult(1.0m);
}
