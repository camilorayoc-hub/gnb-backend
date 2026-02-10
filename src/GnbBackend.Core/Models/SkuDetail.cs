namespace GnbBackend.Core.Models;

public class SkuDetail
{
    public required string Sku { get; init; }
    public List<ConvertedTransaction> Transactions { get; init; } = new();
    public decimal TotalInEur { get; init; }
    public List<string> ConversionWarnings { get; init; } = new();
}