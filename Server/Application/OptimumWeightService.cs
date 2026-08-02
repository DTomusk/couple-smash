using Domain.Entities;

namespace Application;

public interface IOptimumWeightService
{
    Task<IEnumerable<Pairing>> GetOptimumWeightPairingsAsync(IEnumerable<Member> members, CancellationToken cancellationToken = default);
}

public class OptimumWeightService : IOptimumWeightService
{
    public async Task<IEnumerable<Pairing>> GetOptimumWeightPairingsAsync(IEnumerable<Member> members, CancellationToken cancellationToken = default)
    {
        if (members == null || !members.Any())
        {
            return Enumerable.Empty<Pairing>();
        }
    }
}
