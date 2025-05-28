using FotoGen.Domain.Entities;
using FotoGen.Domain.ValueObjects;

namespace FotoGen.Domain.Interfaces
{
    public interface ITrainedModelRepository
    {
        Task<List<TrainModelEntity>> GetByStatusAsync(TrainModelStatus status);
        Task AddAsync(TrainModelEntity entity);
        Task UpdateAsync(TrainModelEntity entity);
    }
}
