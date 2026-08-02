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

    [HttpGet]
    public async Task<IActionResult> GetRandomPairing()
    {
        var pairing = await _service.GetRandomPairingAsync();
        return Ok(pairing);
    }

    [HttpPost(Name = "RatePairing")]
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

    [HttpPost(Name = "ExemptPairing")]
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
}
