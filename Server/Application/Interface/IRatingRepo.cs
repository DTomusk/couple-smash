using Domain.Entities;

namespace Application.Interface;

public interface IRatingRepo
{
    Task CreateRatingAsync(Rating rating, CancellationToken cancellationToken = default);
    Task<IEnumerable<Rating>> GetRatingsAsync(CancellationToken cancellationToken = default);
}
