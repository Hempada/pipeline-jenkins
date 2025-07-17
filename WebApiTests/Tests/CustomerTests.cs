using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;

namespace WebApiTests.Tests
{
    public class CustomerControllerTests : IClassFixture<WebApplicationFactory<WebApi.Program>>
    {
        private readonly HttpClient _client;

        public CustomerControllerTests(WebApplicationFactory<WebApi.Program> factory)
        {
            _client = factory.CreateClient();
        }

        //[Fact]
        public async Task Create_Cliente()
        {
            // 1. Obter o Token Bearer fazendo o login
            var loginResponse = await RealizarLogin();
            var token = await ObterToken(loginResponse);

            // 2. Definir o cliente a ser criado
            var novoCliente = new
            {
                Name = "Casas Bahia",
                Email = "contato@casasbahia.com"
            };

            // Serializa o corpo da requisição em JSON
            var content = new StringContent(JsonConvert.SerializeObject(novoCliente), Encoding.UTF8, "application/json");

            // Adiciona o cabeçalho de autenticação Bearer com o token obtido
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Envia a requisição POST para o endpoint
            var response = await _client.PostAsync("/api/customer", content);

            // Verifica se o status da resposta é 200 OK ou 201 Created
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created, $"Unexpected status code: {response.StatusCode}");

            // Ler o corpo da resposta
            var responseBody = await response.Content.ReadAsStringAsync();

            // Exibir o corpo da resposta se houver necessidade de validação adicional
        }

        private async Task<HttpResponseMessage> RealizarLogin()
        {
            // Dados do usuário para login 
            var usuarioLogin = new
            {
                Username = "pedro1",
                EncryptedSecret = "00000000"
            };

            var content = new StringContent(JsonConvert.SerializeObject(usuarioLogin), Encoding.UTF8, "application/json");

            // Faz a requisição de login
            var response = await _client.PostAsync("/api/auth/login", content);

            // Verifica se o login foi bem-sucedido
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            return response;
        }

        private static async Task<string?> ObterToken(HttpResponseMessage loginResponse)
        {
            // Pega o conteúdo da resposta de login
            var responseBody = await loginResponse.Content.ReadAsStringAsync();

            // Pega o token que está dentro do campo 'token' no corpo da resposta JSON
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseBody);

            // Retorna o token
            return tokenResponse?.Token;
        }

        // Classe para deserializar a resposta de login 
        public class TokenResponse
        {
            public required string Token { get; set; }
        }
    }
}
