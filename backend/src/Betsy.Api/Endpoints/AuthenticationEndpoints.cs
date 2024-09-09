using Betsy.Api.Endpoints.Internal;
using Betsy.Application.Authentication.Commands.Register;
using Betsy.Application.Authentication.Common;
using Betsy.Application.Authentication.Queries.Login;
using Betsy.Contracts.Authentication;
using FluentValidation.Results;
using MediatR;
using ErrorOr;

namespace Betsy.Api.Endpoints;

public class AuthenticationEndpoints : IEndpoints
{
    private const string ContentType = "application/json";
    private const string Tag = "Authentication";
    private const string BaseRoute = "";

    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost($"{BaseRoute}/register", RegisterAsync)
            .WithName("Register")
            .Accepts<RegisterRequest>(ContentType)
            .Produces<AuthenticationResponse>(201).Produces<IEnumerable<ValidationFailure>>(400)
            .WithTags(Tag);

        app.MapPost($"{BaseRoute}/login", LoginAsync)
            .WithName("Login")
            .Produces<AuthenticationResponse>(200).Produces<IEnumerable<ValidationFailure>>(400)
            .WithTags(Tag);
    }

    internal static async Task<IResult> RegisterAsync(RegisterRequest request, ISender sender)
    {
        var command = new RegisterCommand(request.FirstName, request.LastName, request.Email, request.Password);

        var response = await sender.Send(command);

        return response.Match(r => Results.Ok(ToContractsAuthenticationResult(r)), Results.BadRequest);
    }

    internal static async Task<IResult> LoginAsync(ISender sender, LoginRequest request)
    {
        var command = new LoginQuery(request.Email, request.Password);

        var response = await sender.Send(command);

        return response.Match(
            r => Results.Ok(ToContractsAuthenticationResult(r)), Results.BadRequest);
    }

    private static AuthenticationResponse ToContractsAuthenticationResult(AuthenticationResult result)
    {
        return new AuthenticationResponse(result.User.Id, result.User.FirstName, result.User.LastName, result.User.Email, result.Token);
    }
}
