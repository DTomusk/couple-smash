namespace Domain.Entities;

public class Pairing
{
    public Guid Id { get; init; }
    public Guid FirstMemberId { get; init; }
    public Guid SecondMemberId { get; init; }
    public bool IsExempted { get; private set; }
    public decimal CompatibilityRating { get; private set; }
    public int NumberOfRatings { get; private set; }

    public Pairing(Guid firstMemberId, Guid secondMemberId)
    {
        Id = Guid.NewGuid();
        FirstMemberId = firstMemberId;
        SecondMemberId = secondMemberId;
        IsExempted = false;
        CompatibilityRating = 0.0m;
        NumberOfRatings = 0;
    }

    public void Rate(decimal rating)
    {
        if (rating < 0)
            throw new ArgumentException("Rating cannot be negative.", nameof(rating));
        if (rating > 5)
            throw new ArgumentException("Rating cannot exceed 5.", nameof(rating));
        NumberOfRatings++;
        CompatibilityRating = ((CompatibilityRating * (NumberOfRatings - 1)) + rating) / NumberOfRatings;
    }

    // Real couples are exempted from being rated
    public void SetExempted()
    {
        IsExempted = true;
    }
}
