using FotoGen.Domain.Entities;
using FotoGen.Domain.Entities.Models;

namespace FotoGen.Domain.Repositories;

public interface IModelTrainingRepository
{
    Task<List<ModelTraining>> GetByStatusAsync(ModelTrainingStatus trainingStatus, CancellationToken cancel);
    Task CreateAsync(ModelTraining entity, CancellationToken cancel);
    Task UpdateAsync(ModelTraining entity, CancellationToken cancel);
}
