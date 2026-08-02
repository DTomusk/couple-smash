using Application;
using Domain.Entities;

namespace Application.UnitTests;

public class OptimumWeightServiceTests
{
    private readonly IOptimumWeightService _service = new OptimumWeightService();

    #region Simple Test Cases - Null and Empty Inputs

    [Fact]
    public async Task GetOptimumWeightPairingsAsync_WithNullMembers_ReturnsEmpty()
    {
        // Arrange
        IEnumerable<Member>? members = null;
        var pairings = new List<Pairing>();

        // Act
        var result = await _service.GetOptimumWeightPairingsAsync(members, pairings);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetOptimumWeightPairingsAsync_WithEmptyMembers_ReturnsEmpty()
    {
        // Arrange
        var members = new List<Member>();
        var pairings = new List<Pairing>();

        // Act
        var result = await _service.GetOptimumWeightPairingsAsync(members, pairings);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetOptimumWeightPairingsAsync_WithNullPairings_ReturnsEmpty()
    {
        // Arrange
        var members = new List<Member> { new("Alice"), new("Bob") };
        IEnumerable<Pairing>? pairings = null;

        // Act
        var result = await _service.GetOptimumWeightPairingsAsync(members, pairings);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetOptimumWeightPairingsAsync_WithEmptyPairings_ReturnsEmpty()
    {
        // Arrange
        var members = new List<Member> { new("Alice"), new("Bob") };
        var pairings = new List<Pairing>();

        // Act
        var result = await _service.GetOptimumWeightPairingsAsync(members, pairings);

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region Simple Test Cases - Single Pair

    [Fact]
    public async Task GetOptimumWeightPairingsAsync_WithTwoMembersAndOnePairing_ReturnsThatPairing()
    {
        // Arrange
        var alice = new Member("Alice");
        var bob = new Member("Bob");
        var members = new List<Member> { alice, bob };

        var pairing = new Pairing(alice.Id, bob.Id);
        pairing.Rate(5m); // High compatibility
        var pairings = new List<Pairing> { pairing };

        // Act
        var result = await _service.GetOptimumWeightPairingsAsync(members, pairings);

        // Assert
        Assert.Single(result);
        var selectedPairing = result.First();
        Assert.True(
            (selectedPairing.FirstMemberId == alice.Id && selectedPairing.SecondMemberId == bob.Id) ||
            (selectedPairing.FirstMemberId == bob.Id && selectedPairing.SecondMemberId == alice.Id)
        );
    }

    [Fact]
    public async Task GetOptimumWeightPairingsAsync_WithSingleMember_ReturnsEmpty()
    {
        // Arrange
        var alice = new Member("Alice");
        var members = new List<Member> { alice };
        var pairings = new List<Pairing>();

        // Act
        var result = await _service.GetOptimumWeightPairingsAsync(members, pairings);

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region Moderate Test Cases - Multiple Pairs with Different Ratings

    [Fact]
    public async Task GetOptimumWeightPairingsAsync_WithFourMembersAndMultiplePairings_ReturnsOptimalMatching()
    {
        // Arrange
        var alice = new Member("Alice");
        var bob = new Member("Bob");
        var charlie = new Member("Charlie");
        var diana = new Member("Diana");
        var members = new List<Member> { alice, bob, charlie, diana };

        var pairings = new List<Pairing>
        {
            // Alice-Bob: high compatibility (5)
            CreateRatedPairing(alice.Id, bob.Id, 5m),
            // Alice-Charlie: low compatibility (1)
            CreateRatedPairing(alice.Id, charlie.Id, 1m),
            // Bob-Diana: medium compatibility (3)
            CreateRatedPairing(bob.Id, diana.Id, 3m),
            // Charlie-Diana: high compatibility (4)
            CreateRatedPairing(charlie.Id, diana.Id, 4m)
        };

        // Act
        var result = await _service.GetOptimumWeightPairingsAsync(members, pairings);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);

        // Optimal pairing should be Alice-Bob (5) and Charlie-Diana (4) = 9 total
        // NOT Alice-Charlie (1) and Bob-Diana (3) = 4 total
        var aliceBobPairing = resultList.FirstOrDefault(p =>
            (p.FirstMemberId == alice.Id && p.SecondMemberId == bob.Id) ||
            (p.FirstMemberId == bob.Id && p.SecondMemberId == alice.Id));
        var charlieDianaPairing = resultList.FirstOrDefault(p =>
            (p.FirstMemberId == charlie.Id && p.SecondMemberId == diana.Id) ||
            (p.FirstMemberId == diana.Id && p.SecondMemberId == charlie.Id));

        Assert.NotNull(aliceBobPairing);
        Assert.NotNull(charlieDianaPairing);
    }

    [Fact]
    public async Task GetOptimumWeightPairingsAsync_WithThreeMembers_ReturnsOneOptimalPair()
    {
        // Arrange - Odd number of members, only one pair can be made
        var alice = new Member("Alice");
        var bob = new Member("Bob");
        var charlie = new Member("Charlie");
        var members = new List<Member> { alice, bob, charlie };

        var pairings = new List<Pairing>
        {
            CreateRatedPairing(alice.Id, bob.Id, 5m),      // Highest rating
            CreateRatedPairing(alice.Id, charlie.Id, 2m),
            CreateRatedPairing(bob.Id, charlie.Id, 3m)
        };

        // Act
        var result = await _service.GetOptimumWeightPairingsAsync(members, pairings);

        // Assert
        Assert.Single(result);
        var pairing = result.First();
        Assert.True(
            (pairing.FirstMemberId == alice.Id && pairing.SecondMemberId == bob.Id) ||
            (pairing.FirstMemberId == bob.Id && pairing.SecondMemberId == alice.Id)
        );
    }

    #endregion

    #region Complex Test Cases - Larger Groups with Various Ratings

    [Fact]
    public async Task GetOptimumWeightPairingsAsync_WithSixMembersAndVariousRatings_ReturnsOptimalThreePairs()
    {
        // Arrange - Create 6 members
        var members = new List<Member>
        {
            new("Alice"),
            new("Bob"),
            new("Charlie"),
            new("Diana"),
            new("Eve"),
            new("Frank")
        };

        var pairings = new List<Pairing>
        {
            // Strong pairs with high compatibility
            CreateRatedPairing(members[0].Id, members[1].Id, 5m),  // Alice-Bob: 5
            CreateRatedPairing(members[2].Id, members[3].Id, 5m),  // Charlie-Diana: 5
            CreateRatedPairing(members[4].Id, members[5].Id, 5m),  // Eve-Frank: 5

            // Weaker alternatives
            CreateRatedPairing(members[0].Id, members[2].Id, 2m),  // Alice-Charlie: 2
            CreateRatedPairing(members[1].Id, members[3].Id, 1m),  // Bob-Diana: 1
            CreateRatedPairing(members[4].Id, members[0].Id, 1m)   // Eve-Alice: 1
        };

        // Act
        var result = await _service.GetOptimumWeightPairingsAsync(members, pairings);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(3, resultList.Count);

        // Verify all members are paired exactly once
        var pairedMemberIds = new HashSet<Guid>();
        foreach (var pairing in resultList)
        {
            pairedMemberIds.Add(pairing.FirstMemberId);
            pairedMemberIds.Add(pairing.SecondMemberId);
        }
        Assert.Equal(6, pairedMemberIds.Count);
    }

    [Fact]
    public async Task GetOptimumWeightPairingsAsync_WithExemptedCouple_SelectsHigherWeightPairing()
    {
        // Arrange - Exempted couple has high rating, but better alternatives exist
        var alice = new Member("Alice");
        var bob = new Member("Bob");
        var charlie = new Member("Charlie");
        var diana = new Member("Diana");
        var members = new List<Member> { alice, bob, charlie, diana };

        var exemptedPairing = new Pairing(alice.Id, bob.Id);
        exemptedPairing.SetExempted(); // Mark as exempted
        exemptedPairing.Rate(3m); // Lower than best alternative

        var pairings = new List<Pairing>
        {
            exemptedPairing,
            CreateRatedPairing(alice.Id, charlie.Id, 5m), // Higher weight
            CreateRatedPairing(bob.Id, diana.Id, 5m),     // Higher weight
            CreateRatedPairing(charlie.Id, diana.Id, 2m)
        };

        // Act
        var result = await _service.GetOptimumWeightPairingsAsync(members, pairings);

        // Assert
        var resultList = result.ToList();
        // Should prefer Alice-Charlie (5) + Bob-Diana (5) = 10 over Alice-Bob (3) + Charlie-Diana (2) = 5
        Assert.Equal(2, resultList.Count);

        var aliceCharlieExists = resultList.Any(p => 
            (p.FirstMemberId == alice.Id && p.SecondMemberId == charlie.Id) ||
            (p.FirstMemberId == charlie.Id && p.SecondMemberId == alice.Id));
        Assert.True(aliceCharlieExists);
    }

    [Fact]
    public async Task GetOptimumWeightPairingsAsync_WithZeroCompatibilityRatings_ReturnsNoPairings()
    {
        // Arrange - All pairings have zero compatibility (no ratings)
        // Since all weights are 0, the algorithm prefers no pairings (weight 0) over pairing with weight 0
        var alice = new Member("Alice");
        var bob = new Member("Bob");
        var charlie = new Member("Charlie");
        var diana = new Member("Diana");
        var members = new List<Member> { alice, bob, charlie, diana };

        var pairings = new List<Pairing>
        {
            new(alice.Id, bob.Id),      // Default: 0 compatibility
            new(charlie.Id, diana.Id)   // Default: 0 compatibility
        };

        // Act
        var result = await _service.GetOptimumWeightPairingsAsync(members, pairings);

        // Assert
        // With all weights at 0, the algorithm finds leaving members unpaired (weight 0) 
        // equivalent to pairing them, so it returns empty
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetOptimumWeightPairingsAsync_WithPositiveButEqualRatings_ReturnsPairings()
    {
        // Arrange - All pairings have same positive compatibility
        var alice = new Member("Alice");
        var bob = new Member("Bob");
        var charlie = new Member("Charlie");
        var diana = new Member("Diana");
        var members = new List<Member> { alice, bob, charlie, diana };

        var pairings = new List<Pairing>
        {
            CreateRatedPairing(alice.Id, bob.Id, 2m),      // All ratings are 2
            CreateRatedPairing(charlie.Id, diana.Id, 2m)   
        };

        // Act
        var result = await _service.GetOptimumWeightPairingsAsync(members, pairings);

        // Assert
        var resultList = result.ToList();
        // With positive equal weights, pairings are preferred
        Assert.Equal(2, resultList.Count);

        // Verify all members are paired
        var pairedMemberIds = new HashSet<Guid>
        {
            resultList[0].FirstMemberId,
            resultList[0].SecondMemberId,
            resultList[1].FirstMemberId,
            resultList[1].SecondMemberId
        };
        Assert.Equal(4, pairedMemberIds.Count);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public async Task GetOptimumWeightPairingsAsync_WithCancellationToken_CompletesSuccessfully()
    {
        // Arrange
        var alice = new Member("Alice");
        var bob = new Member("Bob");
        var members = new List<Member> { alice, bob };
        var pairing = CreateRatedPairing(alice.Id, bob.Id, 5m);
        var pairings = new List<Pairing> { pairing };
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _service.GetOptimumWeightPairingsAsync(members, pairings, cancellationToken);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task GetOptimumWeightPairingsAsync_WithAllMembersEvenlyMatched_ReturnsValidMatching()
    {
        // Arrange - All pairs have identical ratings
        var members = new List<Member>
        {
            new("A"), new("B"), new("C"), new("D")
        };

        var pairings = new List<Pairing>
        {
            CreateRatedPairing(members[0].Id, members[1].Id, 3m),
            CreateRatedPairing(members[0].Id, members[2].Id, 3m),
            CreateRatedPairing(members[0].Id, members[3].Id, 3m),
            CreateRatedPairing(members[1].Id, members[2].Id, 3m),
            CreateRatedPairing(members[1].Id, members[3].Id, 3m),
            CreateRatedPairing(members[2].Id, members[3].Id, 3m)
        };

        // Act
        var result = await _service.GetOptimumWeightPairingsAsync(members, pairings);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);

        // Verify all members are paired
        var pairedMemberIds = new HashSet<Guid>();
        foreach (var pairing in resultList)
        {
            pairedMemberIds.Add(pairing.FirstMemberId);
            pairedMemberIds.Add(pairing.SecondMemberId);
        }
        Assert.Equal(4, pairedMemberIds.Count);
    }

    #endregion

    #region Helper Methods

    private static Pairing CreateRatedPairing(Guid memberId1, Guid memberId2, decimal rating)
    {
        var pairing = new Pairing(memberId1, memberId2);
        pairing.Rate(rating);
        return pairing;
    }

    #endregion
}
