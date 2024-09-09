using Betsy.Application.Common.Models;

namespace Betsy.Application.Common.Interfaces;

public interface IUserSession
{
    CurrentUser GetCurrentUser();
}