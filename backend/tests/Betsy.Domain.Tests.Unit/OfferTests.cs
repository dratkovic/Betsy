using FluentAssertions;

namespace Betsy.Domain.Tests.Unit
{
    public class OfferTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenValidParameters()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var isSpecialOffer = true;

            // Act
            var offer = new Offer(matchId, isSpecialOffer);

            // Assert
            offer.MatchId.Should().Be(matchId);
            offer.IsSpecialOffer.Should().Be(isSpecialOffer);
            offer.BetTypes.Should().BeEmpty();
        }

        [Fact]
        public void AddBetType_ShouldAddBetType_WhenValidParameters()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var offer = new Offer(matchId);
            var title = "Win";
            var quota = 1.5m;

            // Act
            var betType = offer.AddBetType(title, quota);

            // Assert
            offer.BetTypes.Should().ContainSingle();
            offer.BetTypes.First().Should().Be(betType);
            betType.Title.Should().Be(title);
            betType.Quota.Should().Be(quota);
            betType.OfferId.Should().Be(offer.Id);
            betType.MatchId.Should().Be(matchId);
            betType.IsSpecialOffer.Should().BeFalse();
        }

        [Fact]
        public void AddBetType_ShouldThrowException_WhenBetTypeAlreadyExists()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var offer = new Offer(matchId);
            offer.AddBetType("Win", 1.5m);

            // Act
            Action act = () => offer.AddBetType("Win", 2.0m);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Betting type already exists*");
        }

        [Fact]
        public void RemoveBetType_ShouldRemoveBetType_WhenBetTypeExists()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var offer = new Offer(matchId);
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
            var matchId = Guid.NewGuid();
            var offer = new Offer(matchId);

            // Act
            Action act = () => offer.RemoveBetType("Win");

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Betting type not found*");
        }
    }
}
