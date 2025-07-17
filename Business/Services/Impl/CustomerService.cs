using Business.Models;
using Commons.Data.Results;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Impl
{
    public class CustomerService : ICustomerService
    {
        private ApplicationDbContext Database { get; }

        public CustomerService(ApplicationDbContext database)
        {
            Database = database;
        }

        public async Task<Result> SaveAsync(Guid? id, string name, string email, CancellationToken token = default)
        {
            var alreadyExists = await Database.Customers.FirstOrDefaultAsync(x =>
                x.Name == name && (id == null || id != x.Id), token
            );

            if (alreadyExists is not null)
            {
                return Result.Fail(Error.CustomerAlreadyExists);
            }

            if (id is null)
            {
                var customer = Customer.Create(name, email);

                await Database.AddAsync(customer, token);
            }
            else
            {
                var customer = await Database.Customers.FirstOrDefaultAsync(x => x.Id == id, token);
                if (customer is null)
                {
                    return Result.Fail(Error.CustomerNotFound);
                }

                customer.Update(name, email);
                Database.Update(customer);
            }

            await Database.SaveChangesAsync(token);

            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken token = default)
        {
            var customer = await Database.Customers.FirstOrDefaultAsync(x => x.Id == id, token);
            if (customer is null)
            {
                return Result.Fail(Error.CustomerNotFound);
            }

            Database.Remove(customer);

            await Database.SaveChangesAsync(token);

            return Result.Ok();
        }
    }
}
