using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;

namespace WebApiTests.Tests
{
    public class AuthTests : IClassFixture<WebApplicationFactory<WebApi.Program>>
    {
        private readonly HttpClient _client;

        public AuthTests(WebApplicationFactory<WebApi.Program> factory)
        {
            _client = factory.CreateClient();
        }

        //[Fact]
        public async Task Cadastro_Usuario_Ou_Login()
        {
            var token = await AuthenticateAndGetToken();
            Assert.NotNull(token);
        }

        public async Task<string> AuthenticateAndGetToken()
        {
            var novoUsuario = new
            {
                Name = "Pedro Pedroso",
                Username = "pedro1",
                Email = "pedro3@a.com",
                EncryptedSecret = "00000000"
            };

            var usuarioParaLogin = new
            {
                novoUsuario.Username,
                novoUsuario.EncryptedSecret
            };

            var usuarioExistente = await UsuarioJaCadastrado(novoUsuario.Username);

            HttpResponseMessage loginResponse;
            if (usuarioExistente)
            {
                loginResponse = await RealizarLogin(usuarioParaLogin);
                Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
            }
            else
            {
                var content = new StringContent(JsonConvert.SerializeObject(novoUsuario), Encoding.UTF8, "application/json");
                var cadastroResponse = await _client.PostAsync("/api/auth/register", content);
                Assert.Equal(HttpStatusCode.Created, cadastroResponse.StatusCode);

                loginResponse = await RealizarLogin(usuarioParaLogin);
                Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
            }

            return await ObterToken(loginResponse);
        }

        private async Task<bool> UsuarioJaCadastrado(string username)
        {
            var response = await _client.GetAsync($"/api/auth/userExists/{username}");
            return response.StatusCode == HttpStatusCode.OK;
        }

        private async Task<HttpResponseMessage> RealizarLogin(object usuarioParaLogin)
        {
            var loginContent = new StringContent(JsonConvert.SerializeObject(usuarioParaLogin), Encoding.UTF8, "application/json");
            return await _client.PostAsync("/api/auth/login", loginContent);
        }

        private static async Task<string> ObterToken(HttpResponseMessage loginResponse)
        {
            var responseBody = await loginResponse.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseBody);

            if (tokenResponse != null)
                return tokenResponse.Token;

            throw new InvalidOperationException("TokenResponse é nulo.");
        }

        public class TokenResponse
        {
            public string Token { get; set; } = string.Empty;
        }

    }



}
