namespace Domain.Entities;

public class Rating
{
    public Guid Id { get; init; }
    public Guid PairingId { get; init; }
    public decimal Value { get; init; }
    public DateTime CreatedAt { get; init; }

    private Rating() { }

    public Rating(Guid pairingId, decimal value)
    {
        Id = Guid.NewGuid();
        PairingId = pairingId;
        Value = value;
        CreatedAt = DateTime.UtcNow;
    }
}