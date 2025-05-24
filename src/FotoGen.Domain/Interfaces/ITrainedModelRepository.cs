using FotoGen.Domain.Entities;

namespace FotoGen.Domain.Interfaces
{
    public interface ITrainedModelRepository
    {
        Task<TrainModelEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<TrainModelEntity>> GetAllAsync();
        Task AddAsync(TrainModelEntity entity);
        Task UpdateAsync(TrainModelEntity entity);
        Task DeleteAsync(TrainModelEntity entity);
    }
}
