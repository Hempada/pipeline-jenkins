using Commons.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Controllers;

namespace WebApi.Extensions
{
    public static class ApiControllerBaseExtensions
    {
        public static void AddAccount(this ApiControllerBase Controller, Account Account)
        {
            var identity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, Account.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, Account.Username ?? string.Empty),
                    new Claim(ClaimTypes.WindowsAccountName, Account.Username ?? string.Empty),
                    new Claim(ClaimTypes.Name, Account.Name),
                });
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext();
            httpContext.User = claimsPrincipal;
            Controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }
    }
}
