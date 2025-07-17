using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace WebApiTests.Tests

{
    public class ProgramTests : IClassFixture<WebApplicationFactory<WebApi.Program>>
    {
        private readonly HttpClient _client;

        public ProgramTests(WebApplicationFactory<WebApi.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Application_Should_Start_Successfully()
        {
            // Act
            var response = await _client.GetAsync("/");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
