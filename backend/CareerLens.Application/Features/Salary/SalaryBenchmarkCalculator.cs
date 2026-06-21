using CareerLens.Application.Common.Interfaces;
using CareerLens.Domain.Entities;

namespace CareerLens.Application.Features.Salary;

public record SalaryBenchmarkResult(decimal P25, decimal P50, decimal P75, bool IsInflationAdjusted);

/// <summary>
/// Maaş kayıtlarını TÜFE bazlı enflasyon düzeltmesi uygulayarak percentile'a çevirir.
/// GetSalaryBenchmarkQueryHandler ve CreateRoadmapCommandHandler arasında paylaşılır.
/// </summary>
public static class SalaryBenchmarkCalculator
{
    public static async Task<SalaryBenchmarkResult> CalculateAsync(
        IReadOnlyList<SalaryRecord> records,
        ICpiIndexService cpiIndex,
        CancellationToken ct = default)
    {
        if (records.Count == 0)
            return new SalaryBenchmarkResult(0, 0, 0, false);

        var factorsByMonth = new Dictionary<DateTime, decimal>();
        var adjustedSalaries = new List<decimal>(records.Count);
        foreach (var record in records)
        {
            var month = new DateTime(record.SubmittedAt.Year, record.SubmittedAt.Month, 1);
            if (!factorsByMonth.TryGetValue(month, out var factor))
            {
                factor = await cpiIndex.GetAdjustmentFactorAsync(record.SubmittedAt, ct);
                factorsByMonth[month] = factor;
            }
            adjustedSalaries.Add(record.NetSalary * factor);
        }

        var sorted = adjustedSalaries.OrderBy(s => s).ToList();
        var isAdjusted = factorsByMonth.Values.Any(f => f != 1.0m);

        return new SalaryBenchmarkResult(
            Percentile(sorted, 25),
            Percentile(sorted, 50),
            Percentile(sorted, 75),
            isAdjusted);
    }

    private static decimal Percentile(List<decimal> sortedData, int percentile)
    {
        if (sortedData.Count == 0) return 0;
        var index = (int)Math.Ceiling(percentile / 100.0 * sortedData.Count) - 1;
        return sortedData[Math.Max(0, index)];
    }
}
