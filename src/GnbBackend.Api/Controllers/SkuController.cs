using GnbBackend.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GnbBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkuController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly ILogger<SkuController> _logger;

    public SkuController(ITransactionService transactionService, ILogger<SkuController> logger)
    {
        _transactionService = transactionService;
        _logger = logger;
    }

    [HttpGet("{sku}")]
    public async Task<IActionResult> GetSkuDetail(string sku)
    {
        try
        {
            var detail = await _transactionService.GetSkuDetailAsync(sku);
            return Ok(detail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error for SKU {Sku}", sku);
            return StatusCode(500, new { error = "An error ocurred" });
        }
    }
}