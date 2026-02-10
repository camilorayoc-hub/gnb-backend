using GnbBackend.Core.Models;

namespace GnbBackend.Core.Interfaces;

public interface ITransactionService
{
    Task<SkuDetail> GetSkuDetailAsync(string sku);
}