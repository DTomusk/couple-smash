using Application.Interface;
using Domain.Entities;

namespace Infrastructure.Repos;

public class PairingRepo : IPairingRepo
{
    private readonly AppDbContext _context;

    public PairingRepo(AppDbContext context)
    {
        _context = context;
    }

    public Task<Pairing> GetPairingAsync(Guid pairingId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePairingAsync(Pairing pairing, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task CreatePairingAsync(Pairing pairing, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
