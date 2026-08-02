using Application.DTOs;
using Application.Interface;

namespace Application;

public interface IPairingService
{
    Task RatePairingAsync(Guid pairingId, decimal rating);
    Task<PairingResponse> GetRandomPairingAsync();
    Task ExemptPairingAsync(Guid pairingId);
}

public class PairingService : IPairingService
{
    private readonly IPairingRepo _pairingRepo;

    public PairingService(IPairingRepo pairingRepo)
    {
        _pairingRepo = pairingRepo;
    }

    public async Task ExemptPairingAsync(Guid pairingId)
    {
        var pairing = await _pairingRepo.GetPairingAsync(pairingId);
        if (pairing == null)
            throw new ArgumentException("Pairing not found.", nameof(pairingId));

        pairing.SetExempted();
        await _pairingRepo.UpdatePairingAsync(pairing);
    }

    public Task<PairingResponse> GetRandomPairingAsync()
    {
        throw new NotImplementedException();
    }

    public async Task RatePairingAsync(Guid pairingId, decimal rating)
    {
        var pairing = await _pairingRepo.GetPairingAsync(pairingId);
        if (pairing == null)
            throw new ArgumentException("Pairing not found.", nameof(pairingId));

        if (pairing.IsExempted)
            throw new InvalidOperationException("Cannot rate an exempted pairing.");

        pairing.Rate(rating);
        await _pairingRepo.UpdatePairingAsync(pairing);
    }
}
