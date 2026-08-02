using Application.Interface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repos;

public class PairingRepo : IPairingRepo
{
    private readonly AppDbContext _context;

    public PairingRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Pairing?> GetPairingAsync(Guid pairingId, CancellationToken cancellationToken = default)
    {
        return await _context.Pairings.FindAsync(new object[] { pairingId }, cancellationToken);
    }

    public async Task<IEnumerable<Pairing>> GetPairingsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Pairings.ToListAsync(cancellationToken);
    }

    public async Task UpdatePairingAsync(Pairing pairing, CancellationToken cancellationToken = default)
    {
        _context.Update(pairing);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task CreatePairingAsync(Pairing pairing, CancellationToken cancellationToken = default)
    {
        _context.Add(pairing);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
