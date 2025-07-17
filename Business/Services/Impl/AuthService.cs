using Business.Models;
using Commons.Data.Results;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Impl
{
    public class AuthService : IAuthService
    {
        private ApplicationDbContext Database { get; }

        public AuthService(ApplicationDbContext database)
        {
            Database = database;
        }

        public async Task<Result<Account>> AuthAsync(string username, string password, CancellationToken token = default)
        {
            Account? account = await Database.Accounts
                .Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.Username == username, token);

            if (account is null)
            {
                return Result.Fail(Error.AccountNotFound);
            }

            return Result.Ok(account);
        }

        public async Task<Result<Account>> GetAccountAsync(Guid id, CancellationToken token = default)
        {
            Account? account = await Database.Accounts
                .Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.Id == id, token);

            if (account is null)
            {
                return Result.Fail(Error.AccountNotFound);
            }

            return Result.Ok(account);
        }

        public async Task<Result> RegisterAsync(string name, string username, string email, string password, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return Result.Fail(Error.EmptyRequiedFields);
            }

            Account account = Account.Create(name, username, email, null);

            if (await Database.Accounts.FirstOrDefaultAsync(x => x.Username == username, token) is not null)
            {
                return Result.Fail(Error.UsernameAlreadyExists);
            }

            if (await Database.Accounts.FirstOrDefaultAsync(x => x.Username == username, token) is not null)
            {
                return Result.Fail(Error.UsernameAlreadyExists);
            }

            if (await Database.Accounts.FirstOrDefaultAsync(x => x.Email == email, token) is not null)
            {
                return Result.Fail(Error.EmailAlreadyExists);
            }

            try
            {
                await Database.AddAsync(account, token);
                await Database.SaveChangesAsync(token);
            }
            catch
            {
                return Result.Fail(Error.SaveChangesFailed);
            }

            try
            {
                //TODO: cadastrar usuário e senha em sistema de autenticação externo
            }
            catch
            {
                var undoAccount = await Database.Accounts.FirstOrDefaultAsync(x => x.Username == username, token);
                if (undoAccount is not null)
                {
                    Database.Remove(undoAccount);
                    await Database.SaveChangesAsync(token);
                }

                return Result.Fail(Error.RegisterAuthFailed);
            }

            return Result.Ok();
        }
    }
}
