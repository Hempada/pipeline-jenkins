using Business.Services;
using Commons.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Controllers;
using Xunit;
using Commons.Data.Results;

namespace WebApiTests.Tests.Controllers
{
    public class ProfileControllerTests : ControllerTests
    {
        private readonly ProfileController Controller;
        private readonly IProfileService Service;

        private readonly string Create_Name = "Perfil de testes";
        private readonly IEnumerable<string> Create_Permissions = Permission.All;

        private readonly string Edit_Name = "Perfil de testes - Editado";
        private readonly IEnumerable<string> Edit_Permissions = Enumerable.Empty<string>();

        private Guid? Temp_Id;

        public ProfileControllerTests() : base()
        {
            Controller = new ProfileController();
            Service = ServiceProvider.GetRequiredService<IProfileService>();
        }

        private void Cleanup()
        {
            var trash = Database.Profiles.Where(x => x.Name == Create_Name || x.Name == Edit_Name).ToArray();
            Database.RemoveRange(trash);
            Database.SaveChanges();
        }

        private async Task Create_ReturnsOk_WhenSaveIsSuccessful()
        {
            Cleanup();

            // Create
            var createModel = new SaveProfile(null, Create_Name, Create_Permissions);

            var result = await Controller.Save(Service, createModel, default);
            Assert.IsType<OkResult>(result);

            var storedModel = await Database.Profiles.FirstOrDefaultAsync(x => x.Name == Create_Name);
            Assert.IsType<Business.Models.Profile>(storedModel);
            Assert.Equal(storedModel.Permissions.Count(), Create_Permissions.Count());
            Assert.All(storedModel.Permissions, item =>
            {
                Assert.Contains(Create_Permissions, x => x == item);
            });

            Temp_Id = storedModel.Id;
        }

        private async Task CreateAndUpdate_ReturnsOk_WhenSaveIsSuccessful()
        {
            await Create_ReturnsOk_WhenSaveIsSuccessful();
            Assert.NotNull(Temp_Id);

            var editModel = new SaveProfile(Temp_Id, Edit_Name, Edit_Permissions);
            var result = await Controller.Save(Service, editModel, default);
            Assert.IsType<OkResult>(result);

            var storedModel = await Database.Profiles.FirstOrDefaultAsync(x => x.Name == Create_Name);
            Assert.Null(storedModel);

            storedModel = await Database.Profiles.FirstOrDefaultAsync(x => x.Name == Edit_Name);
            Assert.IsType<Business.Models.Profile>(storedModel);
            Assert.Equal(storedModel.Permissions, Edit_Permissions);
        }

        [Fact]
        public async Task CreateUpdateAndDelete_ReturnsOk_WhenSaveIsSuccessful()
        {
            await CreateAndUpdate_ReturnsOk_WhenSaveIsSuccessful();
            Assert.NotNull(Temp_Id);

            var result = await Controller.Delete(Service, Temp_Id.Value, default);
            Assert.IsType<OkResult>(result);

            var storedModel = await Database.Profiles.FirstOrDefaultAsync(x => x.Name == Create_Name);
            Assert.Null(storedModel);

            storedModel = await Database.Profiles.FirstOrDefaultAsync(x => x.Name == Edit_Name);
            Assert.Null(storedModel);
        }

        [Fact]
        public async Task Delete_ReturnsProfileNotFound_WhenIdIsInvalid()
        {
            Cleanup();

            // Attempt to delete with an invalid ID
            var invalidId = Guid.Empty;
            var result = await Controller.Delete(Service, invalidId, default);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Error[]>(badRequestResult.Value);
            Assert.Contains(errors, e => e.Code == "PROFILE_NOT_FOUND" && e.Description == "Perfil não encontrado.");
        }

        // Implementação do serviço que sempre falha
        private class FailingProfileService : IProfileService
        {
            public Task<Result> SaveAsync(Guid? id, string name, IEnumerable<string> permissions, CancellationToken token = default)
            {
                if (string.IsNullOrEmpty(name))
                {
                    return Task.FromResult(Result.Fail(new Error("INVALID_NAME", "Name is required.")));
                }

                if (permissions == null || !permissions.Any())
                {
                    return Task.FromResult(Result.Fail(new Error("INVALID_PERMISSIONS", "Permissions are required.")));
                }

                return Task.FromResult(Result.Fail(Error.ProfileNotFound));
            }

            public Task<Result> DeleteAsync(Guid id, CancellationToken token = default)
            {
                if (id == Guid.Empty)
                {
                    return Task.FromResult(Result.Fail(Error.ProfileNotFound));
                }

                return Task.FromResult(Result.Fail(Error.ProfileNotFound));
            }
        }
    }
}
