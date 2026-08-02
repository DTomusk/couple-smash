namespace Domain.Entities;

public class Member
{
    public Guid Id { get; init; }
    public string Name { get; init; }

    public Member(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        if (name.Length > 50)
            throw new ArgumentException("Name cannot exceed 50 characters.", nameof(name));

        Id = Guid.NewGuid();
        Name = name.Trim();
    }
}
