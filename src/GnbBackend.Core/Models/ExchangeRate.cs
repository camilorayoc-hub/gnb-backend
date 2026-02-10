namespace GnbBackend.Core.Models;

public class ExchangeRate
{
    public required string From { get; init; }
    public required string To { get; init; }
    public decimal Rate { get; init; }
}