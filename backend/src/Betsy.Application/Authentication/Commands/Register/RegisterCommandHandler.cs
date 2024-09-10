using Betsy.Application.Authentication.Common;
using Betsy.Application.Common.Interfaces;
using Betsy.Application.Common.Interfaces.Repositories;
using Betsy.Domain;
using Betsy.Domain.Common;
using MediatR;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Betsy.Application.Authentication.Commands.Register;

public class RegisterCommandHandler(
    IBetsyDbContext _dbContext,
    IJwtTokenGenerator _jwtTokenGenerator,
    IPasswordHasher _passwordHasher)
        : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var email = command.Email.ToLower();
        if (await _dbContext.Set<User>().AnyAsync(x => x.Email == email, cancellationToken))
        {
            return Error.Conflict(description: "User already exists");
        }

        var hashPasswordResult = _passwordHasher.HashPassword(command.Password);

        if (hashPasswordResult.IsError)
        {
            return hashPasswordResult.Errors;
        }

        var user = new User(
            command.FirstName,
            command.LastName,
            command.Email.ToLower(),
            hashPasswordResult.Value);

        _dbContext.Set<User>().Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}