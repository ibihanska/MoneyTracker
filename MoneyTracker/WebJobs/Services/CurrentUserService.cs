using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MoneyTracker.Application.Common.Interfaces;

namespace WebJobs.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService()
        {
            UserEmail = "webjob@gmail.com";
        }
        public string UserEmail { get; }
    }
}
