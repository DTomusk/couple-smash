using Application.Interface;
using Domain.Entities;

namespace Application;

public interface IMemberService
{
    Task CreateMember(string Name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Member>> GetMembers(CancellationToken cancellationToken = default);
}

public class MemberService : IMemberService
{
    private readonly IMemberRepo _memberRepo;
    private readonly IPairingRepo _pairingRepo;

    public MemberService(IMemberRepo memberRepo, IPairingRepo pairingRepo)
    {
        _memberRepo = memberRepo;
        _pairingRepo = pairingRepo;
    }

    public async Task CreateMember(string Name, CancellationToken cancellationToken = default)
    {
        var existingMember = await _memberRepo.GetMemberByNameAsync(Name, cancellationToken);
        if (existingMember != null)
            throw new ArgumentException("Member with the same name already exists.", nameof(Name));

        var allMembers = await _memberRepo.GetAllMembersAsync(cancellationToken);

        var newMember = new Member(Name);

        // TODO: consider making all these repo methods atomic
        await _memberRepo.CreateMemberAsync(newMember, cancellationToken);

        // TODO: this is orchestrating a pairing concern, which probably shouldn't live in the member service
        foreach (var member in allMembers)
        {
            var pairing = new Pairing(newMember.Id, member.Id);
            await _pairingRepo.CreatePairingAsync(pairing, cancellationToken);
        }
    }

    public async Task<IEnumerable<Member>> GetMembers(CancellationToken cancellationToken = default)
    {
        var members = await _memberRepo.GetAllMembersAsync(cancellationToken);
        return members;
    }
}
