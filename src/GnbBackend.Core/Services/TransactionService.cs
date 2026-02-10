using GnbBackend.Core.Interfaces;
using GnbBackend.Core.Models;
using Microsoft.Extensions.Logging;

namespace GnbBackend.Core.Services;

public class TransactionService : ITransactionService
{
    private readonly IDataRepository _repository;
    private readonly ICurrencyConverter _currencyConverter;
    private readonly ILogger<TransactionService> _logger;

    public TransactionService(IDataRepository repository, ICurrencyConverter currencyConverter, ILogger<TransactionService> logger)
    {
        _repository = repository;
        _currencyConverter = currencyConverter;
        _logger = logger;
    }

    public async Task<SkuDetail> GetSkuDetailAsync(string sku)
    {
        var transactions = await _repository.GetTransactionsBySkuAsync(sku);
        var transactionsList = transactions.ToList();

        if (!transactionsList.Any()) {
            return new SkuDetail
            {
                Sku = sku,
                Transactions = new List<ConvertedTransaction>(),
                TotalInEur = 0m,
                ConversionWarnings = new List<string> { "No transactions found" }
            };
        }

        var convertedTransactions = new List<ConvertedTransaction>();
        var warnings = new List<string>();
        decimal totalInEur = 0m;

        foreach(var transaction in transactionsList){
            var amountInEur = _currencyConverter.Convert(transaction.Amount, transaction.Currency, "EUR");

            if (amountInEur.HasValue){
                totalInEur += amountInEur.Value;

                convertedTransactions.Add(new ConvertedTransaction
                {
                    Sku = transaction.Sku,
                    OriginalAmount = transaction.Amount,
                    OriginalCurrency = transaction.Currency,
                    AmountInEur = amountInEur.Value
                });
            } else {
                var warning = $"Cannot convert {transaction.Amount} {transaction.Currency}";
                warnings.Add(warning);

                convertedTransactions.Add(new ConvertedTransaction
                {
                    Sku = transaction.Sku,
                    OriginalAmount = transaction.Amount,
                    OriginalCurrency = transaction.Currency,
                    AmountInEur = 0m
                });
            }
        }
        totalInEur = Math.Round(totalInEur, 2, MidpointRounding.ToEven);

        return new SkuDetail
        {
            Sku = sku,
            Transactions = convertedTransactions,
            TotalInEur = totalInEur,
            ConversionWarnings = warnings
        };
    }
}
