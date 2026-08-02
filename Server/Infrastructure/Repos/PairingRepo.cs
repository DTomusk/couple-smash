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

    public Task<Pairing> GetPairingAsync(Guid pairingId)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePairingAsync(Pairing pairing)
    {
        throw new NotImplementedException();
    }
}
