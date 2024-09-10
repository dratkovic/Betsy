using FluentAssertions;

namespace Betsy.Domain.Tests.Unit
{
    public class MatchTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenValidParameters()
        {
            // Arrange
            var nameOne = "Team A";
            var description = "Match between Team A and Team B";
            var startsAtUtc = DateTime.UtcNow.AddMinutes(20);
            var sport = Sport.Football;
            var nameTwo = "Team B";
            var corellationId = "12345";

            // Act
            var match = new Match(nameOne, description, startsAtUtc, sport, nameTwo, corellationId);

            // Assert
            match.NameOne.Should().Be(nameOne);
            match.NameTwo.Should().Be(nameTwo);
            match.Description.Should().Be(description);
            match.StartsAtUtc.Should().Be(startsAtUtc);
            match.Sport.Should().Be(sport.ToString());
            match.CorrelationId.Should().Be(corellationId);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenNameOneIsEmpty()
        {
            // Arrange
            var description = "Match between Team A and Team B";
            var startsAtUtc = DateTime.UtcNow.AddMinutes(20);
            var sport = Sport.Football;

            // Act
            Action act = () => new Match(string.Empty, description, startsAtUtc, sport);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Invalid name*");
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenDescriptionIsEmpty()
        {
            // Arrange
            var nameOne = "Team A";
            var startsAtUtc = DateTime.UtcNow.AddMinutes(20);
            var sport = Sport.Football;

            // Act
            Action act = () => new Match(nameOne, string.Empty, startsAtUtc, sport);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Invalid description*");
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenStartsAtUtcIsInThePast()
        {
            // Arrange
            var nameOne = "Team A";
            var description = "Match between Team A and Team B";
            var startsAtUtc = DateTime.UtcNow.AddMinutes(5);
            var sport = Sport.Football;

            // Act
            Action act = () => new Match(nameOne, description, startsAtUtc, sport);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Invalid start date*");
        }
    }
}
