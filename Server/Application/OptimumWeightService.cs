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

        // Map member id to index in member list
        var memberIdToIndex = memberList
            .Select((m, i) => new { m.Id, Index = i })
            .ToDictionary(x => x.Id, x => x.Index);

        // Construct weights matrix
        var weights = new decimal[count, count];
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < count; j++)
            {
                // Self pairings are invalid
                if (i != j)
                {
                    // Get the score of the existing pairing for the members
                    // Make it super low if not present/exempt
                    decimal rating = GetRatingForPairing(memberList[i].Id, memberList[j].Id, pairings);
                    weights[i, j] = rating;
                }
            }
        }

        // Cache the weights matrix so we don't have to recalculate for a given mask
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

            // Start with not pairing the first node with anyone
            var (bestWeight, bestPairs) = SolveMatching(maskWithoutFirst);

            // Try pairing the first node with each other node
            for (int secondNode = firstNode + 1; secondNode < count; secondNode++)
            {
                if ((mask & (1 << secondNode)) != 0)
                {
                    int maskWithoutBoth = maskWithoutFirst ^ (1 << secondNode);
                    var (remainingWeight, remainingPairs) = SolveMatching(maskWithoutBoth);

                    decimal pairWeight = weights[firstNode, secondNode];
                    decimal totalWeight = pairWeight + remainingWeight;

                    if (totalWeight > bestWeight)
                    {
                        bestWeight = totalWeight;
                        bestPairs = new List<(int, int)>(remainingPairs);
                        bestPairs.Add((firstNode, secondNode));
                    }
                }
            }

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

        if (pairing == null)
            return -1000m;

        // Expect exempted pairs haven't been passed in, but protect against it
        if (pairing.IsExempted)
            return -1000m;

        return pairing.CompatibilityRating;
    }
}
