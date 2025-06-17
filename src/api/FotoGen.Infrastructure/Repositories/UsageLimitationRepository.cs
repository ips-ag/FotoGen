using Azure.Data.Tables;
using FotoGen.Domain.Entities.Models;
using FotoGen.Domain.Repositories;

namespace FotoGen.Infrastructure.Repositories
{
    public class UsageLimitationRepository : IUsageLimitationRepository
    {
        private readonly TableClient _tableClient;
        public UsageLimitationRepository(TableClient tableClient)
        {
            _tableClient = tableClient;
            _tableClient.CreateIfNotExists();
        }
        public async Task<UserUsage?> GetUserUsageAsync(string userId, DateOnly date)
        {
            var response = await _tableClient.GetEntityAsync<TableEntity>(userId, date.ToString("yyyy-MM-dd"));
            return MapTableEntityToUserUsage(response.Value);
           
        }
        private TableEntity MapUserUsageToTableEntity(UserUsage userUsage)
        {
            return new TableEntity(userUsage.UserId, userUsage.Date.ToString("yyyy-MM-dd"))
            {
                { nameof(UserUsage.TrainingCount), userUsage.TrainingCount },
                { nameof(UserUsage.PhotoGenerationCount), userUsage.PhotoGenerationCount },
                { nameof(UserUsage.LastUpdate), userUsage.LastUpdate }
            };
        }
        private UserUsage? MapTableEntityToUserUsage(TableEntity? entity)
        {
            if (entity == null) return null;
            if (!TryParseTableEntity(entity, out var userUsage)) return null;
            return userUsage;
        }
        private static bool TryParseTableEntity(TableEntity entity, out UserUsage? userUsage)
        {
            userUsage = null;
            if (string.IsNullOrEmpty(entity.PartitionKey) ||
                string.IsNullOrEmpty(entity.RowKey))
                return false;
            if (!DateOnly.TryParse(entity.RowKey, out var date)) return false;
            userUsage = new UserUsage()
            {
                UserId = entity.PartitionKey,
                Date = date,
                TrainingCount = entity.GetInt32(nameof(UserUsage.TrainingCount)) ?? 0,
                PhotoGenerationCount = entity.GetInt32(nameof(UserUsage.PhotoGenerationCount)) ?? 0,
                LastUpdate = entity.GetDateTime(nameof(UserUsage.LastUpdate)) ?? DateTime.UtcNow
            };
            return true;
        }
        public async Task UpSetAsync(UserUsage userUsage)
        {
            var entity = MapUserUsageToTableEntity(userUsage);
            await _tableClient.UpsertEntityAsync(entity);
        }
    }
}
