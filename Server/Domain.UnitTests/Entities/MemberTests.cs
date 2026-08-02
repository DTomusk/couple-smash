using Domain.Entities;

namespace Domain.UnitTests.Entities;

public class MemberTests
{
    [Fact]
    public void Member_Should_Have_Valid_Id_And_Name()
    {
        // Arrange
        var name = "John Doe";

        // Act
        var member = new Member(name);

        // Assert
        Assert.NotEqual(Guid.Empty, member.Id);
        Assert.Equal(name, member.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData("\t")]
    public void Member_Should_Throw_Exception_For_Null_Or_Empty_Name(string invalidName)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Member(invalidName));
    }

    [Fact]
    public void Member_Should_Throw_Exception_For_Name_Exceeding_Max_Length()
    {
        // Arrange
        var longName = new string('a', 51); // 51 characters
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Member(longName));
    }

    [Fact]
    public void Member_Name_Should_Be_Trimmed()
    {
        // Arrange
        var nameWithSpaces = "   Jane Doe   ";
        // Act
        var member = new Member(nameWithSpaces);
        // Assert
        Assert.Equal("Jane Doe", member.Name);
    }
}
