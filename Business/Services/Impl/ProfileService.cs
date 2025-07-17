using Business.Models;
using Commons.Data.Results;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Impl
{
    public class ProfileService : IProfileService
    {
        private ApplicationDbContext Database { get; }

        public ProfileService(ApplicationDbContext database)
        {
            Database = database;
        }

        public async Task<Result> SaveAsync(Guid? id, string name, IEnumerable<string> permissions, CancellationToken token = default)
        {
            var alreadyExists = await Database.Profiles.FirstOrDefaultAsync(x =>
                x.Name == name && (id == null || id != x.Id), token
            );

            if (alreadyExists is not null)
            {
                return Result.Fail(Error.ProfileAlreadyExists);
            }

            if (id is null)
            {
                var profile = Profile.Create(name, permissions);

                await Database.AddAsync(profile, token);
            }
            else
            {
                var profile = await Database.Profiles.FirstOrDefaultAsync(x => x.Id == id, token);
                if (profile is null)
                {
                    return Result.Fail(Error.ProfileNotFound);
                }

                profile.Update(name, permissions);
                Database.Update(profile);
            }

            await Database.SaveChangesAsync(token);

            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken token = default)
        {
            var associated = await Database.Accounts.FirstOrDefaultAsync(x => x.ProfileId == id, token);
            if (associated is not null)
            {
                return Result.Fail(Error.ProfileAssociated);
            }

            var profile = await Database.Profiles.FirstOrDefaultAsync(x => x.Id == id, token);
            if (profile is null)
            {
                return Result.Fail(Error.ProfileNotFound);
            }

            Database.Remove(profile);

            await Database.SaveChangesAsync(token);

            return Result.Ok();
        }
    }
}
