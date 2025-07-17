using Commons.Data.Results;

namespace Business.Services
{
    public interface IAccountService
    {
        Task<Result> SaveAsync(Guid? id, string name, string username, string email, Guid profileId, CancellationToken token = default);
        Task<Result> DeleteAsync(Guid id, CancellationToken token = default);
    }
}