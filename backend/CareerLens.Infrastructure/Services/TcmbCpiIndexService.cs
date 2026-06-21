using System.Globalization;
using System.Text.Json;
using CareerLens.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CareerLens.Infrastructure.Services;

public class TcmbCpiIndexService : ICpiIndexService
{
    private const string SeriesCode = "TP.FE.OKTG01"; // TÜFE genel endeks (TCMB EVDS)
    private const string CacheKeyPrefix = "cpi:tufe:";
    private static readonly TimeSpan CacheTtl = TimeSpan.FromDays(7);

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICacheService _cache;
    private readonly IConfiguration _config;
    private readonly ILogger<TcmbCpiIndexService> _logger;

    public TcmbCpiIndexService(
        IHttpClientFactory httpClientFactory,
        ICacheService cache,
        IConfiguration config,
        ILogger<TcmbCpiIndexService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache;
        _config = config;
        _logger = logger;
    }

    public async Task<decimal> GetAdjustmentFactorAsync(DateTime fromDate, CancellationToken ct = default)
    {
        try
        {
            var fromMonth = new DateTime(fromDate.Year, fromDate.Month, 1);
            var latestMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

            if (fromMonth >= latestMonth)
                return 1.0m;

            var fromIndex = await GetMonthlyIndexAsync(fromMonth, ct);
            var latestIndex = await GetMonthlyIndexAsync(latestMonth, ct)
                ?? await GetMonthlyIndexAsync(latestMonth.AddMonths(-1), ct);

            if (fromIndex is null || latestIndex is null || fromIndex == 0)
                return 1.0m;

            return Math.Round(latestIndex.Value / fromIndex.Value, 4);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "TCMB TÜFE verisi alınamadı, enflasyon düzeltmesi uygulanmadı.");
            return 1.0m;
        }
    }

    private async Task<decimal?> GetMonthlyIndexAsync(DateTime month, CancellationToken ct)
    {
        var cacheKey = $"{CacheKeyPrefix}{month:yyyy-MM}";
        var cached = await _cache.GetAsync<decimal?>(cacheKey, ct);
        if (cached is not null) return cached;

        var apiKey = _config["Tcmb:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey) || apiKey == "mock")
            return null;

        var firstDay = new DateTime(month.Year, month.Month, 1);
        var lastDay = firstDay.AddMonths(1).AddDays(-1);

        var client = _httpClientFactory.CreateClient("Tcmb");
        var url = $"series={SeriesCode}&startDate={firstDay:dd-MM-yyyy}&endDate={lastDay:dd-MM-yyyy}&type=json&frequency=5";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("key", apiKey);

        using var response = await client.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync(ct);
        var value = ParseLatestIndexValue(json);

        if (value is not null)
            await _cache.SetAsync(cacheKey, value, CacheTtl, ct);

        return value;
    }

    private static decimal? ParseLatestIndexValue(string json)
    {
        var root = JsonDocument.Parse(json).RootElement;
        if (!root.TryGetProperty("items", out var items) || items.GetArrayLength() == 0)
            return null;

        var last = items[items.GetArrayLength() - 1];
        var propertyName = SeriesCode.Replace('.', '_');
        if (!last.TryGetProperty(propertyName, out var valueEl))
            return null;

        var raw = valueEl.ValueKind == JsonValueKind.String ? valueEl.GetString() : valueEl.ToString();
        return decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed)
            ? parsed
            : null;
    }
}
