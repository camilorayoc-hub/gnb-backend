namespace GnbBackend.Core.Models;

public class ConvertedTransaction
{
    public required string Sku { get; init; }
    public decimal OriginalAmount { get; init; }
    public required string OriginalCurrency { get; init; }
    public decimal? AmountInEur { get; init; }
    public string? ConversionError { get; init; }
}