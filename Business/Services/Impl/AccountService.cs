using Business.Models;
using Commons.Data.Results;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Impl
{
    public class AccountService : IAccountService
    {
        private ApplicationDbContext Database { get; }

        public AccountService(ApplicationDbContext database)
        {
            Database = database;
        }

        public async Task<Result> SaveAsync(Guid? id, string name, string username, string email, Guid profileId, CancellationToken token = default)
        {

            var profile = await Database.Profiles.FirstOrDefaultAsync(x => x.Id == profileId, token);
            if (profile is null)
            {
                return Result.Fail(Error.ProfileNotFound);
            }

            var alreadyExists = await Database.Accounts.FirstOrDefaultAsync(x =>
                x.Username == username && (id == null || id != x.Id), token
            );

            if (alreadyExists is not null)
            {
                return Result.Fail(Error.UsernameAlreadyExists);
            }

            alreadyExists = await Database.Accounts.FirstOrDefaultAsync(x =>
                x.Email == email && (id == null || id != x.Id), token
            );

            if (alreadyExists is not null)
            {
                return Result.Fail(Error.EmailAlreadyExists);
            }

            if (id is null)
            {
                var account = Account.Create(name, username, email, profileId);

                await Database.AddAsync(account, token);
            }
            else
            {
                var account = await Database.Accounts.FirstOrDefaultAsync(x => x.Id == id, token);
                if (account is null)
                {
                    return Result.Fail(Error.AccountNotFound);
                }

                account.Update(name, username, email, profileId);
                Database.Update(account);
            }

            await Database.SaveChangesAsync(token);

            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken token = default)
        {
            var account = await Database.Accounts.FirstOrDefaultAsync(x => x.Id == id, token);
            if (account is null)
            {
                return Result.Fail(Error.AccountNotFound);
            }

            Database.Remove(account);

            await Database.SaveChangesAsync(token);

            return Result.Ok();
        }
    }
}
