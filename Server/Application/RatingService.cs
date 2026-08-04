using Application.Interface;
using Domain.Entities;

namespace Application;

public interface IRatingService
{
    Task<IEnumerable<Rating>> GetRatingsAsync();
}

public class RatingService : IRatingService
{
    private readonly IRatingRepo _repo;

    public RatingService(IRatingRepo repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Rating>> GetRatingsAsync()
    {
        return await _repo.GetRatingsAsync();
    }
}
