using Application.Interface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repos;

public class MemberRepo : IMemberRepo
{
    private readonly AppDbContext _context;

    public MemberRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateMemberAsync(Member member, CancellationToken cancellationToken = default)
    {
        _context.Members.Add(member);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Member>> GetAllMembersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Members.ToListAsync(cancellationToken);
    }

    public async Task<Member?> GetMemberByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Members.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }
}
