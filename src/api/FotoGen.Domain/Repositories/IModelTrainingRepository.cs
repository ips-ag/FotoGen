using FotoGen.Domain.Entities.Models;
using FotoGen.Domain.ValueObjects;

namespace FotoGen.Domain.Repositories;

public interface IModelTrainingRepository
{
    Task<List<ModelTraining>> GetByStatusAsync(ModelTrainingStatus trainingStatus);
    Task CreateAsync(ModelTraining entity);
    Task UpdateAsync(ModelTraining entity);
}
