using System.Text.Json;
using System.Text.Json.Serialization;
using GnbBackend.Core.Interfaces;
using GnbBackend.Core.Models;
using Microsoft.Extensions.Configuration;

namespace GnbBackend.Api.Infrastructure;

public class JsonDataRepository : IDataRepository
{
    private readonly string _dataPath;
    private readonly ILogger<JsonDataRepository> _logger;
    private List<Transaction>? _cachedTransactions;
    private List<ExchangeRate>? _cachedRates;

    public JsonDataRepository(IConfiguration configuration, ILogger<JsonDataRepository> logger)
    {
        _dataPath = configuration["DataPath"] ?? "../../Data";
        _logger = logger;
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
    {
        if (_cachedTransactions != null) return _cachedTransactions;

        var filePath = Path.Combine(_dataPath, "transactions.json");
        var json = await File.ReadAllTextAsync(filePath);

        _cachedTransactions = JsonSerializer.Deserialize<List<Transaction>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        }) ?? new List<Transaction>();

        return _cachedTransactions;
    }

    public async Task<IEnumerable<ExchangeRate>> GetAllExchangeRatesAsync()
    {
        if (_cachedRates != null) return _cachedRates;

        var filePath = Path.Combine(_dataPath, "rates.json");
        var json = await File.ReadAllTextAsync(filePath);

        _cachedRates = JsonSerializer.Deserialize<List<ExchangeRate>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        }) ?? new List<ExchangeRate>();

        return _cachedRates;
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsBySkuAsync(string sku)
    {
        var all = await GetAllTransactionsAsync();
        return all.Where(t => t.Sku.Equals(sku, StringComparison.OrdinalIgnoreCase));
    }
}