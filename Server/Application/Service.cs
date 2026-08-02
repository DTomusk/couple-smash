using Application.DTOs;
using Application.Interface;

namespace Application;

public interface IService
{
    Task RatePairingAsync(Guid pairingId, decimal rating);
    Task<PairingResponse> GetRandomPairingAsync();
}

public class Service : IService
{
    private readonly IPairingRepo _pairingRepo;

    public Service(IPairingRepo pairingRepo)
    {
        _pairingRepo = pairingRepo;
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
