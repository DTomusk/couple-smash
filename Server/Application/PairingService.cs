using Application.DTOs;
using Application.Interface;
using Domain.Entities;

namespace Application;

public interface IPairingService
{
    Task RatePairingAsync(Guid pairingId, decimal rating);
    Task<PairingResponse> GetRandomPairingAsync();
    Task<IEnumerable<Pairing>> GetPairingsAsync();
    Task ExemptPairingAsync(Guid pairingId);
}

public class PairingService : IPairingService
{
    private readonly IPairingRepo _pairingRepo;
    private readonly IMemberRepo _memberRepo;

    public PairingService(IPairingRepo pairingRepo, IMemberRepo memberRepo)
    {
        _pairingRepo = pairingRepo;
        _memberRepo = memberRepo;
    }

    public async Task ExemptPairingAsync(Guid pairingId)
    {
        var pairing = await _pairingRepo.GetPairingAsync(pairingId);
        if (pairing == null)
            throw new ArgumentException("Pairing not found.", nameof(pairingId));

        pairing.SetExempted();
        await _pairingRepo.UpdatePairingAsync(pairing);
    }

    public async Task<IEnumerable<Pairing>> GetPairingsAsync()
    {
        return await _pairingRepo.GetPairingsAsync();
    }

    public async Task<PairingResponse> GetRandomPairingAsync()
    {
        var pairing = await _pairingRepo.GetRandomPairingAsync();
        if (pairing == null)
            throw new InvalidOperationException("No pairings available.");

        var members = await _memberRepo.GetMembersByIdsAsync(new[] { pairing.FirstMemberId, pairing.SecondMemberId });
        if (members.Count() != 2)
            throw new InvalidOperationException("One or both members not found.");

        var firstMember = members.FirstOrDefault(x => x.Id == pairing.FirstMemberId);
        if (firstMember == null)
            throw new InvalidOperationException("Member could not be found");

        var secondMember = members.FirstOrDefault(x => x.Id == pairing.SecondMemberId);
        if (secondMember == null)
            throw new InvalidOperationException("Member could not be found");

        return new PairingResponse(pairing.Id, firstMember.Name, secondMember.Name);
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
