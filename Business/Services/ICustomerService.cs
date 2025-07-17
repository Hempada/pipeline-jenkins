using Commons.Data.Results;

namespace Business.Services
{
    public interface ICustomerService
    {
        Task<Result> SaveAsync(Guid? id, string name, string email, CancellationToken token = default);
        Task<Result> DeleteAsync(Guid id, CancellationToken token = default);
    }
}