using Business.Services;
using Commons.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Controllers;
using Commons.Data.Results;
using Xunit;


namespace WebApiTests.Tests.Controllers
{
    public class CustomerControllerTests : ControllerTests
    {
        private readonly CustomerController Controller;
        private readonly ICustomerService Service;

        private readonly string Create_Name = "Test Customer";
        private readonly string Create_Email = "test@example.com";

        private readonly string Edit_Name = "Test Customer - Editado";
        private readonly string Edit_Email = "test_editado@example.com";

        private Guid? Temp_Id;

        public CustomerControllerTests() : base()
        {
            Controller = new CustomerController();
            Service = ServiceProvider.GetRequiredService<ICustomerService>();
        }

        private void Cleanup()
        {
            var trash = Database.Customers.Where(x => x.Name == Create_Name || x.Name == Edit_Name).ToArray();
            Database.RemoveRange(trash);
            Database.SaveChanges();
        }

        [Fact]
        public async Task Create_ReturnsOk_WhenSaveIsSuccessful()
        {
            Cleanup();

            // Arrange
            var model = new SaveCustomer(null, Create_Name, Create_Email);

            // Act
            var result = await Controller.Save(Service, model, default);

            // Assert
            Assert.IsType<OkResult>(result);

            // Verifique se o cliente foi criado
            var createdCustomer = await Database.Customers.FirstOrDefaultAsync(c => c.Name == Create_Name);
            Assert.NotNull(createdCustomer);

            Temp_Id = createdCustomer.Id;
        }

        [Fact]
        public async Task Edit_ReturnsOk_WhenSaveIsSuccessful()
        {
            // Certifique-se de que o cliente foi criado antes de editar
            await Create_ReturnsOk_WhenSaveIsSuccessful();

            // Arrange
            var editModel = new SaveCustomer(Temp_Id, Edit_Name, Edit_Email);

            // Act
            var result = await Controller.Save(Service, editModel, default);

            // Assert
            Assert.IsType<OkResult>(result);

            // Verifique se o cliente foi atualizado
            var editedCustomer = await Database.Customers.FirstOrDefaultAsync(c => c.Id == Temp_Id);
            Assert.NotNull(editedCustomer);
            Assert.Equal(Edit_Name, editedCustomer.Name);
            Assert.Equal(Edit_Email, editedCustomer.Email);
        }

        [Fact]
        public async Task Save_ReturnsBadRequest_WhenServiceFails()
        {
            // Arrange
            var model = new SaveCustomer(Guid.NewGuid(), "", Create_Email); // Nome vazio para simular falha de validação
            var failingService = new FailingCustomerService(); // Usando o serviço que sempre falha

            // Act
            var result = await Controller.Save(failingService, model, default);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenServiceFails()
        {
            // Arrange
            var model = new SaveCustomer(null, "", "test@example.com"); // Nome vazio para simular falha de validação
            var failingService = new FailingCustomerService(); // Usando o serviço que sempre falha

            // Act
            var result = await Controller.Save(failingService, model, default);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task Edit_ReturnsBadRequest_WhenServiceFails()
        {
            // Certifique-se de que o cliente foi criado antes de editar
            await Create_ReturnsOk_WhenSaveIsSuccessful();

            // Arrange
            var editModel = new SaveCustomer(Temp_Id, "", Edit_Email); // Nome vazio para simular falha de validação
            var failingService = new FailingCustomerService(); // Usando o serviço que sempre falha

            // Act
            var result = await Controller.Save(failingService, editModel, default);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenServiceFails()
        {
            // Arrange
            var failingService = new FailingCustomerService(); // Usando o serviço que sempre falha
            var nonExistentCustomerId = Guid.NewGuid();

            // Act
            var result = await Controller.Delete(failingService, nonExistentCustomerId, default);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        // Implementação do serviço que sempre falha
        private class FailingCustomerService : ICustomerService
        {
            public Task<Result> SaveAsync(Guid? id, string name, string email, CancellationToken token = default)
            {
                return Task.FromResult(Result.Fail(Error.AccountNotFound));
            }

            public Task<Result> DeleteAsync(Guid id, CancellationToken token = default)
            {
                return Task.FromResult(Result.Fail(Error.AccountNotFound));
            }
        }
    }
}
