using Betsy.Application.Authentication.Common;
using Betsy.Application.Common.Interfaces;
using Betsy.Application.Common.Interfaces.Repositories;
using Betsy.Domain;
using Betsy.Domain.Common;
using MediatR;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Betsy.Application.Authentication.Queries.Login;

public class LoginQueryHandler(
    IBetsyDbContext _dbContext,
    IJwtTokenGenerator _jwtTokenGenerator,
    IPasswordHasher _passwordHasher)
        : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var email = query.Email.ToLower();
        var user = await _dbContext.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        return user is null || !user.IsCorrectPasswordHash(query.Password, _passwordHasher)
            ? AuthenticationErrors.InvalidCredentials
            : new AuthenticationResult(user, _jwtTokenGenerator.GenerateToken(user));
    }
}