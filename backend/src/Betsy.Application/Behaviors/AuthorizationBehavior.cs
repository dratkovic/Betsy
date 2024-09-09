using System.Reflection;
using Betsy.Application.Common.Authorization;
using Betsy.Application.Common.Interfaces;
using ErrorOr;
using MediatR;

namespace Betsy.Application.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse>(IUserSession userSession)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var authorizationAttributes = request.GetType()
            .GetCustomAttributes<AuthorizeAttribute>()
            .ToList();

        if (authorizationAttributes.Count == 0)
        {
            return await next();
        }

        var currentUser = userSession.GetCurrentUser();

        if (currentUser.IsGuest)
        {
            return (dynamic)Error.Unauthorized(description: "User is forbidden from taking this action");
        }

        return await next();
    }
}
