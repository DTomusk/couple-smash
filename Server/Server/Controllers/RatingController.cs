using Application;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RatingController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRatings()
    {
        return Ok(await _ratingService.GetRatingsAsync());
    }
}
