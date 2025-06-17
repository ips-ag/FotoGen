using FotoGen.Domain.Entities.Models;

namespace FotoGen.Domain.Repositories
{
    public interface IUsageLimitationRepository
    {
        Task<UserUsage?> GetUserUsageAsync(string userId, DateOnly date);
        Task UpSetAsync(UserUsage userUsage);
    }
}
