using AngleSharp.Common;
using Bunit;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebUi.Extensions;

namespace WebApiTests.Tests.Ui
{
    public class UiTests
    {
        public readonly TestContext Context;

        public UiTests()
        {
            Context = new TestContext();
            Context.Services.Configure("http://localhost:5010");

            var inMemorySettings = new Dictionary<string, string?> {
                {"SettingKey", "SettingValue"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            Context.Services.AddSingleton(configuration);
        }
    }
}
