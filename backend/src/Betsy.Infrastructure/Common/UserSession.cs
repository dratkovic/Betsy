using System.Security.Claims;
using Betsy.Application.Common.Interfaces;
using Betsy.Application.Common.Models;
using Microsoft.AspNetCore.Http;
using static System.Boolean;

namespace Betsy.Infrastructure.Common;

public class UserSession(IHttpContextAccessor? _httpContextAccessor) : IUserSession
{
    public CurrentUser GetCurrentUser()
    {
        if (_httpContextAccessor is null)
            return CurrentUser.GuestUser;

        if (!_httpContextAccessor.HttpContext!.User.Claims.Any())
            return CurrentUser.GuestUser;

        var id = GetClaimValues("id")
            .Select(Guid.Parse)
            .First();

        var email = GetClaimValues(ClaimTypes.Email).First();

        return new CurrentUser(id, email);
    }

    private IReadOnlyList<string> GetClaimValues(string claimType)
    {
        return _httpContextAccessor!.HttpContext!.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList();
    }
}
