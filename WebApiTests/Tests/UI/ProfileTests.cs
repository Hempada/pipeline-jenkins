using Bunit;
using Commons.Data.Results;
using Commons.Models;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OData.QueryBuilder.Conventions.AddressingEntities.Query;
using WebUi.Extensions;
using WebUi.Pages.Profiles;
using WebUi.Services;
using Xunit;

namespace WebApiTests.Tests.Ui
{
    public class ProfilesUiTests : UiTests
    {
        public ProfilesUiTests()
        {
            // Configuração do JSInterop para lidar com chamadas de MudBlazor
            Context.JSInterop.SetupVoid("mudPopover.initialize", _ => true);
            Context.JSInterop.SetupVoid("mudPopover.someOtherCall", _ => true); // Exemplo de outra configuração
        }

        [Fact]
        public void ProfilesComponentRendersCorrectly()
        {
            // Act
            var component = Context.RenderComponent<Profiles>();

            // Assert
            Assert.NotNull(component.Find("div")); // Verifica se a div principal está presente
            var method = component.Instance.GetType()
                .GetMethod("TypeNamePlural", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var result = (string)method.Invoke(component.Instance, null);
            Assert.Equal("Perfis", result);
        }

        [Fact]
        public async Task ProfilesComponentHandlesDeleteItemAsync()
        {
            // Arrange
            var mockService = new Mock<IProfileService>();
            mockService.Setup(s => s.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(Result.Ok());

            Context.Services.AddSingleton(mockService.Object);

            var component = Context.RenderComponent<Profiles>();
            var profileId = Guid.NewGuid();

            // Act
            var method = component.Instance.GetType()
                .GetMethod("DeleteItemAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var resultTask = (Task<Result>)method.Invoke(component.Instance, new object[] { profileId });
            var result = await resultTask;

            // Assert
            Assert.True(result.Valid); // Verifica se a operação foi bem-sucedida
            mockService.Verify(s => s.DeleteAsync(profileId, It.IsAny<CancellationToken>()), Times.Once);
        }

    }

    // Implementação manual simplificada para IProfileService
    public class SimpleProfileService : IProfileService
    {
        public Task<Result> DeleteAsync(Guid id, CancellationToken token = default)
        {
            return Task.FromResult(Result.Ok());
        }

        public Task<Result<Profile>> GetAsync(Guid id)
        {
            return Task.FromResult(Result.Ok(new Profile(id, "Test Profile", new List<string>())));
        }

        public Task<Result> SaveAsync(Profile profile)
        {
            return Task.FromResult(Result.Ok());
        }

        public Task<Result> SaveAsync(Guid? id, string name, IEnumerable<string> permissions, CancellationToken token = default)
        {
            return Task.FromResult(Result.Ok());
        }
    }

    // Implementação manual simplificada para IODataService
    public class SimpleODataService : IODataService
    {
        public ValueTask<Result<ODataCountValue<Profile>>> QueryProfileAsync(
            Action<IODataQueryCollection<Profile>> queryFn, string? queryStr = null)
        {
            var profiles = new ODataCountValue<Profile>
            {
                Value = new List<Profile> { new Profile(Guid.NewGuid(), "Admin", new List<string>()) },
                Count = 1
            };
            return ValueTask.FromResult(Result.Ok(profiles));
        }

        public ValueTask<Result<ODataCountValue<Customer>>> QueryCustomerAsync(
            Action<IODataQueryCollection<Customer>> queryFn, string? queryStr = null)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Result<ODataCountValue<Account>>> QueryAccountAsync(
            Action<IODataQueryCollection<Account>> queryFn, string? queryStr = null)
        {
            throw new NotImplementedException();
        }
    }
}
