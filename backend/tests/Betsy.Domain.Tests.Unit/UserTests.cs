using System;
using Betsy.Domain;
using Betsy.Domain.Common;
using Betsy.Domain.Users;
using ErrorOr;
using FluentAssertions;
using Xunit;

namespace Betsy.Domain.Tests.Unit;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WhenValidParameters()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var passwordHash = "hashedpassword";
        var currency = Currencies.Usd;

        // Act
        var user = new User(firstName, lastName, email, passwordHash, currency);

        // Assert
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Email.Should().Be(email);
        user.Wallet.Should().NotBeNull();
        user.Wallet.Currency.Should().Be(currency);
    }

    [Fact]
    public void Constructor_ShouldGenerateNewId_WhenIdIsNotProvided()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var passwordHash = "hashedpassword";

        // Act
        var user = new User(firstName, lastName, email, passwordHash);

        // Assert
        user.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Constructor_ShouldUseProvidedId_WhenIdIsProvided()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var passwordHash = "hashedpassword";
        var userId = Guid.NewGuid();

        // Act
        var user = new User(firstName, lastName, email, passwordHash, id: userId);

        // Assert
        user.Id.Should().Be(userId);
    }

    [Fact]
    public void IsCorrectPasswordHash_ShouldReturnTrue_WhenPasswordIsCorrect()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var passwordHash = "hashedpassword";
        var user = new User(firstName, lastName, email, passwordHash);
        var passwordHasher = new MockPasswordHasher();

        // Act
        var result = user.IsCorrectPasswordHash("password", passwordHasher);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsCorrectPasswordHash_ShouldReturnFalse_WhenPasswordIsIncorrect()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var passwordHash = "hashedpassword";
        var user = new User(firstName, lastName, email, passwordHash);
        var passwordHasher = new MockPasswordHasher();

        // Act
        var result = user.IsCorrectPasswordHash("wrongpassword", passwordHasher);

        // Assert
        result.Should().BeFalse();
    }
}

// Mock implementation of IPasswordHasher for testing purposes
public class MockPasswordHasher : IPasswordHasher
{
    public ErrorOr<string> HashPassword(string password)
    {
        return "hashedpassword";
    }

    public bool IsCorrectPassword(string password, string passwordHash)
    {
        return password == "password" && passwordHash == "hashedpassword";
    }
}

