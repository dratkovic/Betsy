namespace Betsy.Application.Authentication.Common;
using ErrorOr;

public static class AuthenticationErrors
{
    public static readonly Error InvalidCredentials = Error.Validation(
        code: "Authentication.InvalidCredentials",
        description: "Invalid credentials");
}