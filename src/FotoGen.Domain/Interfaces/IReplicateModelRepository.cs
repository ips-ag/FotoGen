using FotoGen.Domain.Entities;

namespace FotoGen.Domain.Interfaces
{
    public interface IReplicateModelRepository
    {
        Task<ReplicateModelEntity> GetByIdAsync(Guid id);
        Task<ReplicateModelEntity> GetByNameAsync(string name);
        Task<IEnumerable<ReplicateModelEntity>> GetAllAsync();
        Task AddAsync(ReplicateModelEntity entity);
        Task UpdateAsync(ReplicateModelEntity entity);
        Task DeleteAsync(ReplicateModelEntity entity);
    }
}
