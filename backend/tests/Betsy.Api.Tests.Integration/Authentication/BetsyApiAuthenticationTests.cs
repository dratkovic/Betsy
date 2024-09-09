using System.Net;
using Betsy.Contracts.Authentication;
using Bogus;
using FluentAssertions;

namespace Betsy.Api.Tests.Integration.Authentication;

[Collection("Betsy tests")]
public class BetsyApiAuthenticationTests : IAsyncLifetime
{
    private readonly BetsyApiFactory _factory;
    private readonly Faker<RegisterRequest> _registerFaker;
    private readonly HttpClient _httpClient;

    public BetsyApiAuthenticationTests(BetsyApiFactory factory)
    {
        _factory = factory;
        _registerFaker = UserFaker.GetRegisterFaker();
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task Register_ShouldReturnAuthenticationResponse_WhenValidUser()
    {
        // Arrange
        var registerRequest = _registerFaker.Generate();

        // Act
        var response = await _httpClient.PostAsJsonAsync("/register", registerRequest);
        var authResponse = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        authResponse!.FirstName.Should().Be(registerRequest.FirstName);
        authResponse.LastName.Should().Be(registerRequest.LastName);
        authResponse.Email.Should().Be(registerRequest.Email.ToLower());
        authResponse.Token.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData("", "cantrell", "jerry@newsy.hr", "P@assword12", "FirstName", "The length of 'First Name' must be at least 3 characters. You entered 0 characters.")]
    [InlineData("jerry", "", "jerry@newsy.hr", "P@assword12", "LastName", "The length of 'Last Name' must be at least 3 characters. You entered 0 characters.")]
    [InlineData("jerry", "cantrell", "jerry", "P@assword12", "Email", "'Email' is not a valid email address.")]
    [InlineData("jerry", "cantrell", "jerry@newsy.hr", "P@ass", "Password", "The length of 'Password' must be at least 8 characters. You entered 5 characters.")]
    public async Task Register_ShouldReturnBadRequest_WhenInvalidUser(string firstName,
        string lastName, string email, string password, string code, string description)
    {
        // Arrange
        var registerRequest = new RegisterRequest(firstName, lastName, email, password);

        // Act
        var response = await _httpClient.PostAsJsonAsync("/register", registerRequest);
        var validationFailures = await response.Content.ReadFromJsonAsync<IEnumerable<ValidationError>>();
        var validationErrors = validationFailures as ValidationError[] ?? validationFailures!.ToArray();
        var error = validationErrors.FirstOrDefault();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error!.code.Should().Be(code);
        error.description.Should().Be(description);
    }

    [Fact]
    public async Task Register_ShouldReturnConflict_WhenUserAlreadyExists()
    {
        // Arrange
        var registerRequest = _registerFaker.Generate();

        // Act
        var response = await _httpClient.PostAsJsonAsync("/register", registerRequest);
        var response2 = await _httpClient.PostAsJsonAsync("/register", registerRequest);
        var validationFailures = await response2.Content.ReadFromJsonAsync<IEnumerable<ValidationError>>();
        var validationErrors = validationFailures as ValidationError[] ?? validationFailures!.ToArray();
        var error = validationErrors.FirstOrDefault();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error!.code.Should().Be("General.Conflict");
        error.description.Should().Be("User already exists");
    }

    [Fact]
    public async Task Login_ShouldReturnAuthenticationResponse_WhenValidUser()
    {
        // Arrange
        var registerRequest = _registerFaker.Generate();
        var loginRequest = new LoginRequest(registerRequest.Email, registerRequest.Password);

        // Act
        var response = await _httpClient.PostAsJsonAsync("/register", registerRequest);
        var response2 = await _httpClient.PostAsJsonAsync("/login", loginRequest);
        var authResponse = await response2.Content.ReadFromJsonAsync<AuthenticationResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response2.StatusCode.Should().Be(HttpStatusCode.OK);
        authResponse!.FirstName.Should().Be(registerRequest.FirstName);
        authResponse.LastName.Should().Be(registerRequest.LastName);
        authResponse.Email.Should().Be(registerRequest.Email.ToLower());
        authResponse.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequest_WhenInvalidUser()
    {
        // Arrange
        var loginRequest = new LoginRequest("test@test.com", "InvalidP@ssword12!");

        // Act
        var response = await _httpClient.PostAsJsonAsync("/login", loginRequest);
        var validationFailures = await response.Content.ReadFromJsonAsync<IEnumerable<ValidationError>>();
        var validationErrors = validationFailures as ValidationError[] ?? validationFailures!.ToArray();
        var error = validationErrors.FirstOrDefault();

        // Assert
        error.Should().NotBeNull();
        error!.code.Should().Be("Authentication.InvalidCredentials");
        error.description.Should().Be("Invalid credentials");
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        _factory.ResetHttpClient();
        await _factory.ResetDataBase();
    }
}