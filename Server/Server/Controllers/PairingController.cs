using Application;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PairingController : ControllerBase
{
    private readonly IPairingService _service;

    public PairingController(IPairingService service)
    {
        _service = service;
    }

    [HttpGet("Random", Name = "GetRandomPairing")]
    public async Task<IActionResult> GetRandomPairing()
    {
        var pairing = await _service.GetRandomPairingAsync();
        return Ok(pairing);
    }

    [HttpGet]
    public async Task<IActionResult> GetPairings()
    {
        var pairings = await _service.GetPairingsAsync();
        return Ok(pairings);
    }

    [HttpPost("Rate", Name = "RatePairing")]
    public async Task<IActionResult> RatePairing([FromBody] RatePairingRequest request)
    {
        try
        {
            await _service.RatePairingAsync(request.PairingId, request.Rating);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("Exempt", Name = "ExemptPairing")]
    public async Task<IActionResult> ExemptPairing(Guid pairingId)
    {
        try
        {
            await _service.ExemptPairingAsync(pairingId);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("Optimal", Name = "GetOptimalPairings")]
    public async Task<IActionResult> GetOptimalPairings()
    {
        var pairings = await _service.GetOptimalPairingsAsync();
        return Ok(pairings);
    }
}
