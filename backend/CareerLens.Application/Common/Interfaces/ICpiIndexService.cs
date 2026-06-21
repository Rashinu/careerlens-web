namespace CareerLens.Application.Common.Interfaces;

public interface ICpiIndexService
{
    /// <summary>
    /// Verilen tarihten bugüne TÜFE bazlı enflasyon düzeltme çarpanını döner
    /// (örn. 1.18 => o tarihteki 100 TL bugün 118 TL'ye eşdeğer).
    /// Veri sağlanamazsa 1m (düzeltme yok) döner.
    /// </summary>
    Task<decimal> GetAdjustmentFactorAsync(DateTime fromDate, CancellationToken ct = default);
}
