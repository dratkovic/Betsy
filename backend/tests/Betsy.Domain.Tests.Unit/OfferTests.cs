using FluentAssertions;

namespace Betsy.Domain.Tests.Unit
{
    public class OfferTests
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

            // Act
            var offer = new Offer(nameOne, description, startsAtUtc, sport, nameTwo);

            // Assert
            offer.NameOne.Should().Be(nameOne);
            offer.NameTwo.Should().Be(nameTwo);
            offer.Description.Should().Be(description);
            offer.StartsAtUtc.Should().Be(startsAtUtc);
            offer.Sport.Should().Be(sport.ToString());
            offer.BetTypes.Should().BeEmpty();
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenNameOneIsEmpty()
        {
            // Arrange
            var description = "Match between Team A and Team B";
            var startsAtUtc = DateTime.UtcNow.AddMinutes(20);
            var sport = Sport.Football;

            // Act
            Action act = () => new Offer(string.Empty, description, startsAtUtc, sport);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Invalid name (Parameter 'nameOne')");
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenDescriptionIsEmpty()
        {
            // Arrange
            var nameOne = "Team A";
            var startsAtUtc = DateTime.UtcNow.AddMinutes(20);
            var sport = Sport.Football;

            // Act
            Action act = () => new Offer(nameOne, string.Empty, startsAtUtc, sport);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Invalid description (Parameter 'description')");
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenStartsAtUtcIsNotTenMinutesFromUtcNow()
        {
            // Arrange
            var nameOne = "Team A";
            var description = "Match between Team A and Team B";
            var startsAtUtc = DateTime.UtcNow.AddMinutes(5);
            var sport = Sport.Football;

            // Act
            Action act = () => new Offer(nameOne, description, startsAtUtc, sport);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Invalid start date (Parameter 'startsAtUtc')*");
        }

        [Fact]
        public void AddBetType_ShouldAddBetType_WhenValidParameters()
        {
            // Arrange
            var offer = new Offer("Team A", "Match between Team A and Team B", DateTime.UtcNow.AddMinutes(20), Sport.Football);
            var title = "Win";
            var quota = 1.5m;

            // Act
            var betType = offer.AddBetType(title, quota);

            // Assert
            offer.BetTypes.Should().ContainSingle();
            offer.BetTypes.First().Should().Be(betType);
            betType.Title.Should().Be(title);
            betType.Quota.Should().Be(quota);
        }

        [Fact]
        public void AddBetType_ShouldThrowException_WhenBetTypeAlreadyExists()
        {
            // Arrange
            var offer = new Offer("Team A", "Match between Team A and Team B", DateTime.UtcNow.AddMinutes(20), Sport.Football);
            offer.AddBetType("Win", 1.5m);

            // Act
            Action act = () => offer.AddBetType("Win", 2.0m);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Betting type already exists (Parameter '_betTypes')");
        }

        [Fact]
        public void RemoveBetType_ShouldRemoveBetType_WhenBetTypeExists()
        {
            // Arrange
            var offer = new Offer("Team A", "Match between Team A and Team B", DateTime.UtcNow.AddMinutes(20), Sport.Football);
            offer.AddBetType("Win", 1.5m);

            // Act
            offer.RemoveBetType("Win");

            // Assert
            offer.BetTypes.Should().BeEmpty();
        }

        [Fact]
        public void RemoveBetType_ShouldThrowException_WhenBetTypeDoesNotExist()
        {
            // Arrange
            var offer = new Offer("Team A", "Match between Team A and Team B", DateTime.UtcNow.AddMinutes(20), Sport.Football);

            // Act
            Action act = () => offer.RemoveBetType("Win");

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Betting type not found*");
        }
    }
}
