using Commons.Data.Results;

namespace Business.Services
{
    public interface IProfileService
    {
        Task<Result> SaveAsync(Guid? id, string name, IEnumerable<string> permissions, CancellationToken token = default);
        Task<Result> DeleteAsync(Guid id, CancellationToken token = default);
    }
}