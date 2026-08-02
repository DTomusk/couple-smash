using Domain.Entities;

namespace Application.Interface;

public interface IPairingRepo
{
    Task<Pairing> GetPairingAsync(Guid pairingId);
    Task UpdatePairingAsync(Pairing pairing);
}
