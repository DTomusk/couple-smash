namespace Domain.Entities;

public class Member
{
    public Guid Id { get; init; }
    public string Name { get; init; }

    public Member(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}
