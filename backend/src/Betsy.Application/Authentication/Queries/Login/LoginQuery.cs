using Betsy.Application.Authentication.Common;
using MediatR;
using ErrorOr;

namespace Betsy.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;