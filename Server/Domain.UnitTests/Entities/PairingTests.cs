using Domain.Entities;

namespace Domain.UnitTests.Entities;

public class PairingTests
{
    [Fact]
    public void Pairing_Should_Have_Valid_Ids_And_Default_Values()
    {
        // Arrange
        var firstMemberId = Guid.NewGuid();
        var secondMemberId = Guid.NewGuid();

        // Act
        var pairing = new Pairing(firstMemberId, secondMemberId);

        // Assert
        Assert.NotEqual(Guid.Empty, pairing.Id);
        Assert.Equal(firstMemberId, pairing.FirstMemberId);
        Assert.Equal(secondMemberId, pairing.SecondMemberId);
        Assert.False(pairing.IsExempted);
        Assert.Equal(0.0m, pairing.CompatibilityRating);
        Assert.Equal(0, pairing.NumberOfRatings);
    }

    [Fact]
    public void Pairing_Should_Calculate_CompatibilityRating_Correctly()
    {
        // Arrange
        var pairing = new Pairing(Guid.NewGuid(), Guid.NewGuid());
        // Act
        pairing.Rate(4.0m);
        pairing.Rate(5.0m);
        pairing.Rate(3.0m);
        // Assert
        Assert.Equal(3, pairing.NumberOfRatings);
        Assert.Equal((4.0m + 5.0m + 3.0m) / 3, pairing.CompatibilityRating);
    }

    [Fact]
    public void Pairing_Should_Throw_Exception_For_Negative_Rating()
    {
        // Arrange
        var pairing = new Pairing(Guid.NewGuid(), Guid.NewGuid());
        // Act & Assert
        Assert.Throws<ArgumentException>(() => pairing.Rate(-1.0m));
    }

    [Fact]
    public void Pairing_Should_Throw_Exception_For_Rating_Exceeding_Max()
    {
        // Arrange
        var pairing = new Pairing(Guid.NewGuid(), Guid.NewGuid());
        // Act & Assert
        Assert.Throws<ArgumentException>(() => pairing.Rate(6.0m));
    }
}
