using FotoGen.Domain.Entities;
using FotoGen.Domain.ValueObjects;

namespace FotoGen.Domain.Interfaces
{
    public interface ITrainedModelRepository
    {
        Task<TrainModelEntity> GetByIdAsync(Guid id);
        Task<List<TrainModelEntity>> GetByStatusAsync(TrainModelStatus status);
        Task<IEnumerable<TrainModelEntity>> GetAllAsync();
        Task AddAsync(TrainModelEntity entity);
        Task UpdateAsync(TrainModelEntity entity);
        Task DeleteAsync(TrainModelEntity entity);
    }
}
