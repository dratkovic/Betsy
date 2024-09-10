using FluentAssertions;

namespace Betsy.Domain.Tests.Unit
{
    public class TicketTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenValidParameters()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var ticketAmount = 100m;

            // Act
            var ticket = new Ticket(userId, ticketAmount);

            // Assert
            ticket.UserId.Should().Be(userId);
            ticket.TicketAmount.Should().Be(ticketAmount);
            ticket.Vig.Should().Be(5m);
            ticket.Stake.Should().Be(95m);
            ticket.TotalQuota.Should().Be(1);
            ticket.ContainsSpecialOffer.Should().BeFalse();
            ticket.OfferSelections.Should().BeEmpty();
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenNoBetTypeAdded()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var ticketAmount = 100m;
            var ticket = new Ticket(userId, ticketAmount);
            
            // Act
            var result = ticket.IsValid();

            // Assert
            result.IsError.Should().BeTrue();
            result.Errors.First().Description.Should().Be("Ticket must contain at least one offer selection");
        }

        [Fact]
        public void AddOfferSelection_ShouldAddSelection_WhenValidBetType()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var ticket = new Ticket(userId, 100m);
            var betType = new BetType("Win", 1.5m, Guid.NewGuid(), Guid.NewGuid(), false);

            // Act
            var result = ticket.AddOfferSelection(betType);

            // Assert
            result.IsError.Should().BeFalse();
            ticket.OfferSelections.Should().ContainSingle();
            ticket.TotalQuota.Should().Be(1.5m);
            ticket.PotentialPayout.Should().Be(142.5m);
        }

        [Fact]
        public void AddOfferSelection_ShouldReturnError_WhenBetTypeQuotaIsLessThanOne()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var ticket = new Ticket(userId, 100m);
            var betType = new BetType("Win", 0.5m, Guid.NewGuid(), Guid.NewGuid(), false);

            // Act
            var result = ticket.AddOfferSelection(betType);

            // Assert
            result.IsError.Should().BeTrue();
            result.Errors.First().Description.Should().Be("Invalid bet type");
        }

        [Fact]
        public void AddOfferSelection_ShouldReturnError_WhenBetTypeAlreadyExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var ticket = new Ticket(userId, 100m);
            var betType = new BetType("Win", 1.5m, Guid.NewGuid(), Guid.NewGuid(), false);
            ticket.AddOfferSelection(betType);

            // Act
            var result = ticket.AddOfferSelection(betType);

            // Assert
            result.IsError.Should().BeTrue();
            result.Errors.First().Description.Should().Be("Ticket already contains selection from offer");
        }

        [Fact]
        public void AddOfferSelection_ShouldReturnError_WhenMixingNormalAndSpecialOffersForSameMatch()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var matchId = Guid.NewGuid();
            var ticket = new Ticket(userId, 100m);
            var normalBetType = new BetType("Win", 1.5m, Guid.NewGuid(), matchId, false);
            var specialBetType = new BetType("Special Win", 2.0m, Guid.NewGuid(), matchId, true);
            ticket.AddOfferSelection(normalBetType);

            // Act
            var result = ticket.AddOfferSelection(specialBetType);

            // Assert
            result.IsError.Should().BeTrue();
            result.Errors.First().Description.Should().Be("Cannot mix normal and special offer for the same match");
        }

        [Fact]
        public void AddOfferSelection_ShouldReturnError_WhenAddingMoreThanOneSpecialOffer()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var ticket = new Ticket(userId, 100m);
            var specialBetType1 = new BetType("Special Win 1", 2.0m, Guid.NewGuid(), Guid.NewGuid(), true);
            var specialBetType2 = new BetType("Special Win 2", 2.5m, Guid.NewGuid(), Guid.NewGuid(), true);
            ticket.AddOfferSelection(specialBetType1);

            // Act
            var result = ticket.AddOfferSelection(specialBetType2);

            // Assert
            result.IsError.Should().BeTrue();
            result.Errors.First().Description.Should().Be("Only one special offer is allowed in the ticket");
        }

        [Fact]
        public void IsValid_ShouldReturnSuccess_WhenTicketIsValid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var ticket = new Ticket(userId, 100m);
            var betType1 = new BetType("Win 1", 1.2m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType2 = new BetType("Win 2", 1.3m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType3 = new BetType("Win 3", 1.4m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType4 = new BetType("Win 4", 1.5m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType5 = new BetType("Win 5", 1.6m, Guid.NewGuid(), Guid.NewGuid(), false);
            ticket.AddOfferSelection(betType1);
            ticket.AddOfferSelection(betType2);
            ticket.AddOfferSelection(betType3);
            ticket.AddOfferSelection(betType4);
            ticket.AddOfferSelection(betType5);

            // Act
            var result = ticket.IsValid();

            // Assert
            result.IsError.Should().BeFalse();
        }

        [Fact]
        public void IsValid_ShouldReturnError_WhenTicketContainsSpecialOfferAndLessThanFiveRegularBetTypes()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var ticket = new Ticket(userId, 100m);
            var specialBetType = new BetType("Special Win", 2.0m, Guid.NewGuid(), Guid.NewGuid(), true);
            var betType1 = new BetType("Win 1", 1.2m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType2 = new BetType("Win 2", 1.3m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType3 = new BetType("Win 3", 1.4m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType4 = new BetType("Win 4", 1.5m, Guid.NewGuid(), Guid.NewGuid(), false);
            ticket.AddOfferSelection(specialBetType);
            ticket.AddOfferSelection(betType1);
            ticket.AddOfferSelection(betType2);
            ticket.AddOfferSelection(betType3);
            ticket.AddOfferSelection(betType4);

            // Act
            var result = ticket.IsValid();

            // Assert
            result.IsError.Should().BeTrue();
            result.Errors.First().Description.Should().Be("Ticket must contain at least 5 regular bet types with quota greater than 1.1");
        }
    }
}
