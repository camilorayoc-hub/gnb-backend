namespace GnbBackend.Core.Models;

public class Transaction
{
    public required string Sku { get; init; }
    public decimal Amount { get; init; }
    public required string Currency { get; init; }
}