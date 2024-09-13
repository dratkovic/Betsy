using FluentAssertions;
using System.Net.Sockets;

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
            var sBetType = new BetType("Win", 1.5m, Guid.NewGuid(), Guid.NewGuid(), false);
            var ticket = Ticket.Create(userId, ticketAmount, [sBetType]);
            
            // Assert
            ticket.Value.UserId.Should().Be(userId);
            ticket.Value.TicketAmount.Should().Be(ticketAmount);
            ticket.Value.Vig.Should().Be(5m);
            ticket.Value.Stake.Should().Be(95m);
            ticket.Value.TotalQuota.Should().Be(1.5m);
            ticket.Value.ContainsSpecialOffer.Should().BeFalse();
            ticket.Value.OfferSelections.Should().NotBeEmpty();
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenNoBetTypeAdded()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var ticketAmount = 100m;


            // Act
            var ticket = Ticket.Create(userId, ticketAmount);

            // Assert
            ticket.IsError.Should().BeTrue();
            ticket.Errors.First().Description.Should().Be("Ticket must contain at least one offer selection");
        }

        [Fact]
        public void AddOfferSelection_ShouldAddSelection_WhenValidBetType()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var betType = new BetType("Win", 1.5m, Guid.NewGuid(), Guid.NewGuid(), false);
            var ticket = Ticket.Create(userId, 100m, [betType]);

            // Act
            var betType2 = new BetType("Win", 1.5m, Guid.NewGuid(), Guid.NewGuid(), false);
            var result = ticket.Value.AddOfferSelection(betType2);

            // Assert
            result.IsError.Should().BeFalse();
        }

        [Fact]
        public void AddOfferSelection_ShouldReturnError_WhenBetTypeQuotaIsLessThanOne()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var betType = new BetType("Win", 0.5m, Guid.NewGuid(), Guid.NewGuid(), false);

            // Act
            var result = Ticket.Create(userId, 100m, [betType]);

            // Assert
            result.IsError.Should().BeTrue();
            result.Errors.First().Description.Should().Be("Invalid bet type");
        }

        [Fact]
        public void AddOfferSelection_ShouldReturnError_WhenBetTypeAlreadyExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var betType = new BetType("Win", 1.5m, Guid.NewGuid(), Guid.NewGuid(), false);
            var ticket = Ticket.Create(userId, 100m, [betType]);
            
            // Act
            var result = ticket.Value.AddOfferSelection(betType);

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
            var normalBetType = new BetType("Win", 1.5m, Guid.NewGuid(), matchId, false);
            var betType1 = new BetType("Win 1", 1.2m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType2 = new BetType("Win 2", 1.3m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType3 = new BetType("Win 3", 1.4m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType4 = new BetType("Win 4", 1.5m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType5 = new BetType("Win 5", 1.6m, Guid.NewGuid(), Guid.NewGuid(), false);

            var ticket = Ticket.Create(userId, 100m, [normalBetType, betType1,betType2,betType3,betType4,betType5]);

            var specialBetType = new BetType("Special Win", 2.0m, Guid.NewGuid(), matchId, true);
            
            // Act
            var result = ticket.Value.AddOfferSelection(specialBetType);

            // Assert
            result.IsError.Should().BeTrue();
            result.Errors.First().Description.Should().Be("Cannot mix normal and special offer for the same match");
        }

        [Fact]
        public void AddOfferSelection_ShouldReturnError_WhenAddingMoreThanOneSpecialOffer()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var specialBetType1 = new BetType("Special Win 1", 2.0m, Guid.NewGuid(), Guid.NewGuid(), true);
            var betType1 = new BetType("Win 1", 1.2m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType2 = new BetType("Win 2", 1.3m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType3 = new BetType("Win 3", 1.4m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType4 = new BetType("Win 4", 1.5m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType5 = new BetType("Win 5", 1.6m, Guid.NewGuid(), Guid.NewGuid(), false);
            var ticket = Ticket.Create(userId, 100m, [specialBetType1, betType1, betType2, betType3, betType4, betType5]);

            var specialBetType2 = new BetType("Special Win 2", 2.5m, Guid.NewGuid(), Guid.NewGuid(), true);
            
            // Act
            var result = ticket.Value.AddOfferSelection(specialBetType2);

            // Assert
            result.IsError.Should().BeTrue();
            result.Errors.First().Description.Should().Be("Only one special offer is allowed in the ticket");
        }

        [Fact]
        public void IsValid_ShouldReturnSuccess_WhenTicketIsValid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            
            var betType1 = new BetType("Win 1", 1.2m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType2 = new BetType("Win 2", 1.3m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType3 = new BetType("Win 3", 1.4m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType4 = new BetType("Win 4", 1.5m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType5 = new BetType("Win 5", 1.6m, Guid.NewGuid(), Guid.NewGuid(), false);

            // Act
            var ticket = Ticket.Create(userId, 100m, [betType1, betType2, betType3, betType4, betType5]);

            // Assert
            ticket.IsError.Should().BeFalse();
        }

        [Fact]
        public void IsValid_ShouldReturnError_WhenTicketContainsSpecialOfferAndLessThanFiveRegularBetTypes()
        {
            // Arrange
            var userId = Guid.NewGuid();
            
            var specialBetType = new BetType("Special Win", 2.0m, Guid.NewGuid(), Guid.NewGuid(), true);
            var betType1 = new BetType("Win 1", 1.2m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType2 = new BetType("Win 2", 1.3m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType3 = new BetType("Win 3", 1.4m, Guid.NewGuid(), Guid.NewGuid(), false);
            var betType4 = new BetType("Win 4", 1.5m, Guid.NewGuid(), Guid.NewGuid(), false);
            

            // Act
            var result = Ticket.Create(userId, 100m, [specialBetType,betType1, betType2,betType3,betType4]);

            // Assert
            result.IsError.Should().BeTrue();
            result.Errors.First().Description.Should().Be("Ticket must contain at least 5 regular bet types with quota greater than 1.1");
        }
    }
}
