using Business.Services;
using Commons.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration) : ApiControllerBase
    {
        private readonly IConfiguration Configuration = configuration;

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(
            [FromServices] IAuthService authService,
            [FromBody] RegisterUser model,
            CancellationToken token = default)
        {
            var result = await authService.RegisterAsync(model.Name, model.UserName,
                model.Email, model.EncryptedSecret, token);

            if (result.Valid)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login(
            [FromServices] IAuthService authService,
            [FromBody] AuthenticateUser model)
        {
            try
            {
                var result = await authService.AuthAsync(model.UserName, model.EncryptedSecret);

                if (result is null || !result.Valid || result.Data is null)
                {
                    return Unauthorized();
                }

                return Ok(
                    new Session(
                        result.Data.ToCommons(),
                        await GetToken(result.Data)
                    )
                );
            }
            catch (Exception)
            {
                return BadRequest("Ocorreu um erro interno no servidor.");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("get-account")]
        public async Task<IActionResult> GetAccount(
            [FromServices] IAuthService authService)
        {
            var accountId = GetCurrentAccountId();

            var result = await authService.GetAccountAsync(accountId);

            if (result is null || !result.Valid || result.Data is null)
            {
                return Unauthorized();
            }

            return Ok(
                new Session(
                    result.Data.ToCommons(),
                    await GetToken(result.Data)
                )
            );
        }

        private async Task<string> GetToken(Business.Models.Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value ?? string.Empty);

            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Sid, account.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, account.Username ?? string.Empty),
                new Claim(ClaimTypes.WindowsAccountName, account.Username ?? string.Empty),
                new Claim(ClaimTypes.Name, account.Name),
            });

            if (!int.TryParse(Configuration.GetSection("AppSettings:Expires").Value, out int expires))
            {
                expires = 24;
            }

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddHours(expires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(descriptor);

            return await Task.Run<string>(() => { return tokenHandler.WriteToken(token); });
        }
    }
}
