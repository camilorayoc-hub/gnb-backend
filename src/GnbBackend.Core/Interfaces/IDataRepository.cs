using GnbBackend.Core.Models;

namespace GnbBackend.Core.Interfaces;

public interface IDataRepository
{
    Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
    Task<IEnumerable<ExchangeRate>> GetAllExchangeRatesAsync();
    Task<IEnumerable<Transaction>> GetTransactionsBySkuAsync(string sku);
}