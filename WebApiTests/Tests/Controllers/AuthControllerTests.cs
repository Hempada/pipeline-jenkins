using Business;
using Business.Services;
using Business.Services.Impl;
using Commons.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using WebApi.Controllers;
using WebApi.Extensions;
using Xunit;

namespace WebApiTests.Tests.Controllers
{
    public class AuthControllerTests : ControllerTests
    {
        private static string? RegisteredUserName;
        private static string? RegisteredPassword;
        private readonly AuthController Controller;
        private readonly IAuthService AuthService;

        public AuthControllerTests() : base()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            serviceCollection.AddScoped<IAuthService, AuthService>();

            Controller = new AuthController(Builder.Configuration);
            AuthService = ServiceProvider.GetRequiredService<IAuthService>();
        }

        private void Cleanup()
        {
            var trash = Database.Accounts.Where(x => x.Email == "test@example.com").ToArray();
            Database.RemoveRange(trash);
            Database.SaveChanges();
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenRegistrationIsSuccessful()
        {
            Cleanup();
            var model = new RegisterUser("Test User", "pedro1", "test@example.com", "encryptedPassword");
            var result = await Controller.Register(AuthService, model);
            Assert.IsType<OkResult>(result);
            RegisteredUserName = model.UserName;
            RegisteredPassword = model.EncryptedSecret;
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenRegistrationFails()
        {
            Cleanup();
            var model = new RegisterUser("Test User", "", "test@example.com", "encryptedPassword");
            var result = await Controller.Register(AuthService, model);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorArray = Assert.IsType<Commons.Data.Results.Error[]>(badRequestResult.Value);
            Assert.Contains(errorArray, e => e.Code == "EMPTY_REQUIRED_FIELDS" && e.Description == "Campos obrigatórios não informados.");
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenLoginIsSuccessful()
        {
            await Register_ReturnsOk_WhenRegistrationIsSuccessful();
            var loginModel = new AuthenticateUser(RegisteredUserName!, RegisteredPassword!);
            var result = await Controller.Login(AuthService, loginModel);
            Assert.IsType<OkObjectResult>(result);
            var session = ((OkObjectResult)result).Value as Session;
            Assert.NotNull(session);
            Assert.NotNull(session!.Account);
            Assert.NotNull(session.Token);
            Assert.Equal(session.Account.Username, RegisteredUserName);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenLoginFails()
        {
            Cleanup();
            var model = new AuthenticateUser("wronguser", "wrongpassword");
            var result = await Controller.Login(AuthService, model);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetAccount_ReturnsOk_WhenAccountIsFound()
        {
            await Register_ReturnsOk_WhenRegistrationIsSuccessful();
            var loginModel = new AuthenticateUser(RegisteredUserName!, RegisteredPassword!);
            var loginResult = await Controller.Login(AuthService, loginModel);
            Assert.IsType<OkObjectResult>(loginResult);

            var session = ((OkObjectResult)loginResult).Value as Session;
            Assert.NotNull(session);
            Assert.NotNull(session!.Account);
            Assert.NotNull(session.Token);

            Controller.AddAccount(session.Account);

            var result = await Controller.GetAccount(AuthService);
            Assert.NotNull(result);
            if (result is OkObjectResult okResult)
            {
                session = okResult.Value as Session;
                Assert.NotNull(session);
                Assert.NotNull(session.Account);
                Assert.NotNull(session.Token);
                Assert.Equal(session.Account.Username, RegisteredUserName);
            }
            else
            {
                Assert.Fail($"Expected OkObjectResult but got {result.GetType().Name}");
            }
        }

        [Fact]
        public async Task GetAccount_ReturnsUnauthorized_WhenAccountNotFound()
        {
            Cleanup();
            var claims = new List<Claim> { new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var user = new ClaimsPrincipal(identity);
            Controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var result = await Controller.GetAccount(AuthService);
            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
