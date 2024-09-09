using Betsy.Domain;

namespace Betsy.Application.Authentication.Common;

public record AuthenticationResult(
    User User,
    string Token);