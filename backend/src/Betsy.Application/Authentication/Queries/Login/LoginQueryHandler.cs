using Betsy.Application.Authentication.Common;
using Betsy.Application.Common.Interfaces;
using Betsy.Application.Common.Interfaces.Repositories;
using Betsy.Domain.Common;
using MediatR;
using ErrorOr;

namespace Betsy.Application.Authentication.Queries.Login;

public class LoginQueryHandler(
    IJwtTokenGenerator _jwtTokenGenerator,
    IPasswordHasher _passwordHasher,
    IUserRepository userRepository)
        : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(query.Email, cancellationToken);

        return user is null || !user.IsCorrectPasswordHash(query.Password, _passwordHasher)
            ? AuthenticationErrors.InvalidCredentials
            : new AuthenticationResult(user, _jwtTokenGenerator.GenerateToken(user));
    }
}