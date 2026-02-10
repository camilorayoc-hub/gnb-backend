using GnbBackend.Core.Models;
using GnbBackend.Core.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace GnbBackend.Tests;

public class CurrencyConverterTests
{
    [Fact]
    public void Convert_SameCurrency_ReturnsAmount()
    {
        var rates = new List<ExchangeRate>();
        var converter = new CurrencyConverter(rates, NullLogger<CurrencyConverter>.Instance);

        var result = converter.Convert(100m, "EUR", "EUR");

        Assert.NotNull(result);
        Assert.Equal(100m, result.Value);
    }

    [Fact]
    public void Convert_DirectPath_Works()
    {
        var rates = new List<ExchangeRate> { new() { From = "USD", To = "EUR", Rate = 0.736m } };
        var converter = new CurrencyConverter(rates, NullLogger<CurrencyConverter>.Instance);

        var result = converter.Convert(10m, "USD", "EUR");

        Assert.NotNull(result);
        Assert.Equal(7.36m, result.Value);
    }

    [Fact]
    public void Convert_MultiStep_Works()
    {
        var rates = new List<ExchangeRate> { 
            new() { From = "SEK", To = "USD", Rate = 0.558m },
            new() { From = "USD", To = "EUR", Rate = 0.736m }
        };
        var converter = new CurrencyConverter(rates, NullLogger<CurrencyConverter>.Instance);

        var result = converter.Convert(10m, "SEK", "EUR");

        Assert.NotNull(result);
        Assert.Equal(4.11m, result.Value);
    }

    [Fact]
    public void Convert_NoPath_ReturnsNull()
    {
        var rates = new List<ExchangeRate> { 
            new() { From = "USD", To = "EUR", Rate = 0.736m }
        };
        var converter = new CurrencyConverter(rates, NullLogger<CurrencyConverter>.Instance);

        var result = converter.Convert(10m, "BRL", "EUR");

        Assert.Null(result);
    }

    [Fact]
    public void Convert_BankersRounding_Works()
    {
        var rates = new List<ExchangeRate> { 
            new() { From = "TEST", To = "EUR", Rate = 0.1005m }
        };
        var converter = new CurrencyConverter(rates, NullLogger<CurrencyConverter>.Instance);

        var result = converter.Convert(10m, "TEST", "EUR");

        Assert.NotNull(result);
        Assert.Equal(1.00m, result.Value);
    }

    [Fact]
    public void Convert_NegativeAmount_Works()
    {
        var rates = new List<ExchangeRate> { 
            new() { From = "USD", To = "EUR", Rate = 0.736m }
        };
        var converter = new CurrencyConverter(rates, NullLogger<CurrencyConverter>.Instance);

        var result = converter.Convert(-10m, "USD", "EUR");

        Assert.NotNull(result);
        Assert.Equal(-7.36m, result.Value);
    }
}