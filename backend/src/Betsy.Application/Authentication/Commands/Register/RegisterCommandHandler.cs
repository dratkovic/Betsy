using Betsy.Application.Authentication.Common;
using Betsy.Application.Common.Interfaces;
using Betsy.Application.Common.Interfaces.Repositories;
using Betsy.Domain;
using Betsy.Domain.Common;
using MediatR;
using ErrorOr;

namespace Betsy.Application.Authentication.Commands.Register;

public class RegisterCommandHandler(
    IJwtTokenGenerator _jwtTokenGenerator,
    IPasswordHasher _passwordHasher,
    IUserRepository userRepository,
    IUnitOfWork _unitOfWork)
        : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsByEmailAsync(command.Email, cancellationToken))
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

        await userRepository.AddAsync(user);
        await _unitOfWork.CommitChangesAsync(cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}