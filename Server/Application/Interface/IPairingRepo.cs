using Domain.Entities;

namespace Application.Interface;

public interface IPairingRepo
{
    Task<Pairing?> GetPairingAsync(Guid pairingId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Pairing>> GetPairingsAsync(CancellationToken cancellationToken = default);
    Task CreatePairingAsync(Pairing pairing, CancellationToken cancellationToken = default);
    Task UpdatePairingAsync(Pairing pairing, CancellationToken cancellationToken = default);
    Task<Pairing?> GetRandomPairingAsync(CancellationToken cancellationToken = default);
}
