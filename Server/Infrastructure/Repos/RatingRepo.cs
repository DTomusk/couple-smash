using Application.Interface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repos;

public class RatingRepo : IRatingRepo
{
    private readonly AppDbContext _context;

    public RatingRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateRatingAsync(Rating rating, CancellationToken cancellationToken = default)
    {
        _context.Add(rating);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Rating>> GetRatingsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Ratings.ToListAsync(cancellationToken);
    }
}
