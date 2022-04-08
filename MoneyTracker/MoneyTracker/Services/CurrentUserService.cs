using System.Security.Claims;
using MoneyTracker.Application.Common.Interfaces;

namespace MoneyTracker.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserEmail = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
        }

        public string UserEmail { get; }
    }
}
