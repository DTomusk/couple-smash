using Application.DTOs;
using Application.Interface;
using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Application;

public interface IPairingService
{
    Task RatePairingAsync(Guid pairingId, decimal rating);
    Task<PairingResponse> GetRandomPairingAsync();
    Task<IEnumerable<Pairing>> GetPairingsAsync();
    Task ExemptPairingAsync(Guid pairingId);
    Task<IEnumerable<Pairing>> GetOptimalPairingsAsync();
}

public class PairingService : IPairingService
{
    private readonly IPairingRepo _pairingRepo;
    private readonly IMemberRepo _memberRepo;
    private readonly IOptimumWeightService _weightService;
    private readonly IMemoryCache _cache;

    private const string CACHE_KEY_OPTIMAL_PAIRINGS = "optimal_pairings";
    private const int CACHE_DURATION_MINUTES = 10;

    public PairingService(IPairingRepo pairingRepo,
        IMemberRepo memberRepo,
        IOptimumWeightService weightService,
        IMemoryCache cache)
    {
        _pairingRepo = pairingRepo;
        _memberRepo = memberRepo;
        _weightService = weightService;
        _cache = cache;
    }

    public async Task ExemptPairingAsync(Guid pairingId)
    {
        var pairing = await _pairingRepo.GetPairingAsync(pairingId);
        if (pairing == null)
            throw new ArgumentException("Pairing not found.", nameof(pairingId));

        pairing.SetExempted();
        await _pairingRepo.UpdatePairingAsync(pairing);
    }

    public async Task<IEnumerable<Pairing>> GetOptimalPairingsAsync()
    {
        if (_cache.TryGetValue(CACHE_KEY_OPTIMAL_PAIRINGS, out IEnumerable<Pairing> cachedPairings))
        {
            return cachedPairings;
        }

        var pairings = await _pairingRepo.GetNonExemptedPairingsAsync();
        var members = await _memberRepo.GetAllMembersAsync();

        // TODO: run algorithm to find optimal pairings based on ratings
        var optimalPairings = await _weightService.GetOptimumWeightPairingsAsync(members, pairings);

        _cache.Set(CACHE_KEY_OPTIMAL_PAIRINGS, optimalPairings, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

        return optimalPairings;
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
