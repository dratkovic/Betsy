using System.Net;
using Betsy.Contracts.Authentication;
using Betsy.Contracts.Common;
using Betsy.Contracts.Offer;
using Betsy.Contracts.Ticket;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Betsy.Api.Tests.Integration.Tickets;

[Collection("Betsy tests")]
public class BetsyApiTicketEndpointsTests : IAsyncLifetime
{
    private readonly BetsyApiFactory _factory;
    private readonly Faker<RegisterRequest> _faker;
    private readonly HttpClient _httpClient;
    private readonly List<OfferResponse> _offers = new();

    public BetsyApiTicketEndpointsTests(BetsyApiFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
        _faker = UserFaker.GetRegisterFaker();
    }

    [Fact]
    public async Task GetTickets_ShouldReturnPaginationResult_WhenTicketsExist()
    {
        // Arrange
        var authUser = await RegisterUser();
        var offers = await _factory.GetOffers();
        var validOffersBettingTypes = offers.Where(x => !x.IsSpecialOffer)
            .Select(x => x.BettingTypes)
            .Take(5)
            .ToList();

        var validOffers = validOffersBettingTypes.Select(x => x.First().Id).ToList();

        var createTicketReq = new CreateTicketRequest(1000, validOffers);
        var ticketCreateResponse = await _httpClient.PostAsJsonAsync("/tickets", createTicketReq);
        var ticketResponse = await ticketCreateResponse.Content.ReadFromJsonAsync<TicketResponse>();

        // Act
        var response = await _httpClient.GetAsync("/tickets");
        var tickets = await response.Content.ReadFromJsonAsync<PaginationResult<TicketResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        tickets.Should().NotBeNull();
        tickets!.Data.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetTickets_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange

        // Act
        var response = await _httpClient.GetAsync($"/tickets");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetTicket_ShouldReturnTicketResponse_WhenTicketExists()
    {
        // Arrange
        var authUser = await RegisterUser();
        var offers = await _factory.GetOffers();
        var validOffersBettingTypes = offers.Where(x => !x.IsSpecialOffer)
            .Select(x => x.BettingTypes)
            .Take(5)
            .ToList();

        var validOffers = validOffersBettingTypes.Select(x => x.First().Id).ToList();

        var createTicketReq = new CreateTicketRequest(1000, validOffers);
        var ticketCreateResponse = await _httpClient.PostAsJsonAsync("/tickets", createTicketReq);
        var ticketResponse = await ticketCreateResponse.Content.ReadFromJsonAsync<TicketResponse>();

        // Act
        var response = await _httpClient.GetAsync($"/tickets/{ticketResponse!.Id}");
        var ticket = await response.Content.ReadFromJsonAsync<PaginationResult<TicketResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        ticket.Should().NotBeNull();
    }


    [Fact]
    public async Task GetTicket_ShouldReturnNotFound_WhenTicketDoesNotExist()
    {
        // Arrange
        var authUser = await RegisterUser();
        var ticketId = Guid.NewGuid();

        // Act
        var response = await _httpClient.GetAsync($"/tickets/{ticketId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetTicket_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        var ticketId = Guid.NewGuid();

        // Act
        var response = await _httpClient.GetAsync($"/tickets/{ticketId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateTicket_ShouldReturnCreatedResponse_WhenValidRequest()
    {
        // Arrange
        var authUser = await RegisterUser();
        var offers = await _factory.GetOffers();
        var validOffersBettingTypes = offers.Where(x => !x.IsSpecialOffer)
            .Select(x => x.BettingTypes)
            .Take(5)
            .ToList();

        var validOffers = validOffersBettingTypes.Select(x => x.First().Id).ToList();

        // Act
        var createTicketReq = new CreateTicketRequest(1000, validOffers);
        var ticketCreateResponse = await _httpClient.PostAsJsonAsync("/tickets", createTicketReq);
        var ticketResponse = await ticketCreateResponse.Content.ReadFromJsonAsync<TicketResponse>();

        // Assert
        ticketCreateResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        ticketResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateTicket_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        var offers = await _factory.GetOffers();
        var validOffersBettingTypes = offers.Where(x => !x.IsSpecialOffer)
            .Select(x => x.BettingTypes)
            .Take(5)
            .ToList();

        var validOffers = validOffersBettingTypes.Select(x => x.First().Id).ToList();

        // Act
        var createTicketReq = new CreateTicketRequest(1000, validOffers);
        var ticketCreateResponse = await _httpClient.PostAsJsonAsync("/tickets", createTicketReq);
        //var ticketResponse = await ticketCreateResponse.Content.ReadFromJsonAsync<TicketResponse>();

        // Assert
        ticketCreateResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }


    [Fact]
    public async Task CreateTicket_ShouldReturnBadRequest_WhenSelectedBetTypesFromSameOffer()
    {
        // Arrange
        var authUser = await RegisterUser();
        var offers = await _factory.GetOffers();
        var validOffersBettingTypes = offers.Where(x => !x.IsSpecialOffer)
            .SelectMany(x => x.BettingTypes)
            .Take(5)
            .ToList();

        var invalidOffers = validOffersBettingTypes.Select(x => x.Id).ToList();

        // Act
        var createTicketReq = new CreateTicketRequest(1000, invalidOffers); // Invalid bet types
        var ticketCreateResponse = await _httpClient.PostAsJsonAsync("/tickets", createTicketReq);
        var validationFailures = await ticketCreateResponse.Content.ReadFromJsonAsync<IEnumerable<ValidationError>>();
        var validationErrors = validationFailures as ValidationError[] ?? validationFailures!.ToArray();
        var error = validationErrors.FirstOrDefault();

        // Assert
        ticketCreateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Should().NotBeNull();
        error!.code.Should().Be("General.Validation");
        error.description.Should().Contain("Ticket already contains selection from offer");
    }

 

    [Fact]
    public async Task CreateTicket_ShouldReturnBadRequest_WhenSelectedBetTypesIdsIsEmpty()
    {
        // Arrange
        var authUser = await RegisterUser();

        // Act
        var createTicketReq = new CreateTicketRequest(1000, new List<Guid>()); // Invalid bet types
        var ticketCreateResponse = await _httpClient.PostAsJsonAsync("/tickets", createTicketReq);
        var validationFailures = await ticketCreateResponse.Content.ReadFromJsonAsync<IEnumerable<ValidationError>>();
        var validationErrors = validationFailures as ValidationError[] ?? validationFailures!.ToArray();
        var error = validationErrors.FirstOrDefault();

        // Assert
        ticketCreateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Should().NotBeNull();
        error!.code.Should().Be("SelectedBetTypesIds");
        error.description.Should().Contain("must not be empty");
    }

    [Fact]
    public async Task CreateTicket_ShouldReturnBadRequest_WhenSelectedMoreThanOneSpecialOffer()
    {
        // Arrange
        var authUser = await RegisterUser();
        var offers = await _factory.GetOffers();
        var validOffersBettingTypes = offers.Where(x => x.IsSpecialOffer)
            .Select(x => x.BettingTypes)
            .Take(5)
            .ToList();

        var invalidOffers = validOffersBettingTypes.Select(x => x.First().Id).ToList();

        // Act
        var createTicketReq = new CreateTicketRequest(1000, invalidOffers); // Invalid bet types
        var ticketCreateResponse = await _httpClient.PostAsJsonAsync("/tickets", createTicketReq);
        var validationFailures = await ticketCreateResponse.Content.ReadFromJsonAsync<IEnumerable<ValidationError>>();
        var validationErrors = validationFailures as ValidationError[] ?? validationFailures!.ToArray();
        var error = validationErrors.FirstOrDefault();

        // Assert
        ticketCreateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Should().NotBeNull();
        error!.code.Should().Be("General.Validation");
        error.description.Should().Contain("Only one special offer is allowed in the ticket");
    }

    [Fact]
    public async Task CreateTicket_ShouldReturnBadRequest_WhenSelectedSpecialOfferAndNormalForSameMatch()
    {
        // Arrange
        var authUser = await RegisterUser();
        var offers = await _factory.GetOffers();
        var specialOffersBettingTypes = offers.Where(x => x.IsSpecialOffer)
            .Select(x => x.BettingTypes)
            .Take(1)
            .ToList();

        var normalOffersBettingTypes = offers.Where(x => !x.IsSpecialOffer)
            .Select(x => x.BettingTypes)
            .Take(5)
            .ToList();

        var invalidOffers = specialOffersBettingTypes.Select(x => x.First().Id).Take(1).ToList();
        invalidOffers.AddRange(normalOffersBettingTypes.Select(x => x.First().Id).Take(3).ToList());
        // Act
        var createTicketReq = new CreateTicketRequest(1000, invalidOffers); // Invalid bet types
        var ticketCreateResponse = await _httpClient.PostAsJsonAsync("/tickets", createTicketReq);
        var validationFailures = await ticketCreateResponse.Content.ReadFromJsonAsync<IEnumerable<ValidationError>>();
        var validationErrors = validationFailures as ValidationError[] ?? validationFailures!.ToArray();
        var error = validationErrors.FirstOrDefault();

        // Assert
        ticketCreateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Should().NotBeNull();
        error!.code.Should().Be("General.Validation");
        error.description.Should().Contain("Cannot mix normal and special offer for the same match");
    }

    [Fact]
    public async Task CreateTicket_ShouldReturnBadRequest_WhenSelectedSpecialOfferWithoutFiveOtherSelections()
    {
        // Arrange
        var authUser = await RegisterUser();
        var offers = await _factory.GetOffers();
        var specialOffersBettingTypes = offers.Where(x => x.IsSpecialOffer)
            .Select(x => x.BettingTypes)
            .TakeLast(1)
            .ToList();

        var normalOffersBettingTypes = offers.Where(x => !x.IsSpecialOffer)
            .Select(x => x.BettingTypes)
            .Take(5)
            .ToList();

        var invalidOffers = specialOffersBettingTypes.Select(x => x.First().Id).Take(1).ToList();
        invalidOffers.AddRange(normalOffersBettingTypes.Select(x => x.First().Id).Take(3).ToList());
        // Act
        var createTicketReq = new CreateTicketRequest(1000, invalidOffers); // Invalid bet types
        var ticketCreateResponse = await _httpClient.PostAsJsonAsync("/tickets", createTicketReq);
        var validationFailures = await ticketCreateResponse.Content.ReadFromJsonAsync<IEnumerable<ValidationError>>();
        var validationErrors = validationFailures as ValidationError[] ?? validationFailures!.ToArray();
        var error = validationErrors.FirstOrDefault();

        // Assert
        ticketCreateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Should().NotBeNull();
        error!.code.Should().Be("General.Validation");
        error.description.Should().Contain("Ticket must contain at least 5 regular bet types with quota greater than 1.1");
    }

    private async Task<AuthenticationResponse> RegisterUser()
    {
        var registerRequest = _faker.Generate();
        var registerResponse = await _httpClient.PostAsJsonAsync("/register", registerRequest);
        var authResponse = await registerResponse.Content.ReadFromJsonAsync<AuthenticationResponse>();

        _httpClient.AddAuthorizationToken(authResponse!.Token);

        return authResponse!;
    }



    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        _factory.ResetHttpClient();
        await _factory.ResetDataBase();
    }
}
