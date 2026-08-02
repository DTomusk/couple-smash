using Application;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PairingController : ControllerBase
{
    private readonly IService _service;

    public PairingController(IService service)
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
}
