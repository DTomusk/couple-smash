using Domain.Entities;

namespace Application;

public interface IOptimumWeightService
{
    Task<IEnumerable<Pairing>> GetOptimumWeightPairingsAsync(IEnumerable<Member> members, IEnumerable<Pairing> pairings, CancellationToken cancellationToken = default);
}

public class OptimumWeightService : IOptimumWeightService
{
    public async Task<IEnumerable<Pairing>> GetOptimumWeightPairingsAsync(IEnumerable<Member> members, IEnumerable<Pairing> pairings, CancellationToken cancellationToken = default)
    {
        if (members == null || !members.Any())
        {
            return Enumerable.Empty<Pairing>();
        }

        if (pairings == null || !pairings.Any())
        {
            return Enumerable.Empty<Pairing>();
        }

        var memberList = members.ToList();
        int count = memberList.Count;

        var memberIdToIndex = memberList
            .Select((m, i) => new { m.Id, Index = i })
            .ToDictionary(x => x.Id, x => x.Index);

        // Construct weights matrix
        var weights = new decimal[count, count];
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < count; j++)
            {
                decimal rating = GetRatingForPairing(memberList[i].Id, memberList[j].Id, pairings);
            }
        }

        var memo = new Dictionary<int, (decimal weight, List<(int, int)> pairs)>();

        (decimal maxWeight, List<(int, int)> selectedPairs) SolveMatching(int mask)
        {
            if (mask == 0)
                return (0, new List<(int, int)>());

            if (memo.TryGetValue(mask, out var cachedResult))
                return cachedResult;

            int firstNode = 0;
            while ((mask & (1 << firstNode)) == 0)
                firstNode++;

            int maskWithoutFirst = mask ^ (1 << firstNode);

            var (bestWeight, bestPairs) = SolveMatching(maskWithoutFirst);

            memo[mask] = (bestWeight, bestPairs);
            return (bestWeight, bestPairs);
        }

        int fullMask = (1 << count) - 1;
        var (_, selectedPairs) = SolveMatching(fullMask);
        return pairings.Where(p => selectedPairs.Any(sp =>
            (memberIdToIndex[p.FirstMemberId] == sp.Item1 && memberIdToIndex[p.SecondMemberId] == sp.Item2) ||
            (memberIdToIndex[p.FirstMemberId] == sp.Item2 && memberIdToIndex[p.SecondMemberId] == sp.Item1)));
    }

    private decimal GetRatingForPairing(Guid memberId1, Guid memberId2, IEnumerable<Pairing> pairings)
    {
        var pairing = pairings.FirstOrDefault(p =>
            (p.FirstMemberId == memberId1 && p.SecondMemberId == memberId2) ||
            (p.FirstMemberId == memberId2 && p.SecondMemberId == memberId1));

        // E.g. for existing couples (those who are exempted), compatibility is 0
        return pairing?.CompatibilityRating ?? 0m;
    }
}
