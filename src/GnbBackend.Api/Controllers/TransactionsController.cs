using GnbBackend.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GnbBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly IDataRepository _repository;
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(IDataRepository repository, ILogger<TransactionsController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var transactions = await _repository.GetAllTransactionsAsync();
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving transactions");
            return StatusCode(500, new { error = "An error ocurred" });
        }
    }
}