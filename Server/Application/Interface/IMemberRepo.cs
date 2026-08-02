using Domain.Entities;

namespace Application.Interface;

public interface IMemberRepo
{
    Task<Member?> GetMemberByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Member>> GetAllMembersAsync(CancellationToken cancellationToken = default);
    Task CreateMemberAsync(Member member, CancellationToken cancellationToken = default);
}
