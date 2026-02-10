using GnbBackend.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GnbBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatesController : ControllerBase
{
    private readonly IDataRepository _repository;
    private readonly ILogger<RatesController> _logger;

    public RatesController(IDataRepository repository, ILogger<RatesController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var rates = await _repository.GetAllExchangeRatesAsync();
            return Ok(rates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving rates");
            return StatusCode(500, new { error = "An error ocurred" });
        }
    }
}