using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MoneyTracker.Application.Common.Interfaces;

namespace WebJobs.Services
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
