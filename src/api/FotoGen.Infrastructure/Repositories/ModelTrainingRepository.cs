using FotoGen.Domain.Entities.Models;
using FotoGen.Domain.Repositories;
using FotoGen.Domain.ValueObjects;

namespace FotoGen.Infrastructure.Repositories;

public class ModelTrainingRepository : IModelTrainingRepository
{
    private readonly string _csvFilePath;
    private readonly Lock _fileLock = new();

    public ModelTrainingRepository(string csvFilePath)
    {
        _csvFilePath = csvFilePath;

        // Ensure directory exists
        string? directory = Path.GetDirectoryName(_csvFilePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Create file with headers if it doesn't exist
        if (!File.Exists(_csvFilePath))
        {
            File.WriteAllText(
                _csvFilePath,
                "Id,ModelName,UserEmail,UserName,ImageUrl,TriggerWord,Status,CreatedAt,SuccessedAt,CanceledUrl\n");
        }
    }

    public async Task CreateAsync(ModelTraining entity)
    {
        string line = CsvHelper.ConvertEntityToCsvLine(entity);

        await Task.Run(() =>
        {
            lock (_fileLock)
            {
                File.AppendAllText(_csvFilePath, line + Environment.NewLine);
            }
        });
    }

    public async Task<List<ModelTraining>> GetByStatusAsync(ModelTrainingStatus trainingStatus)
    {
        return await Task.Run(() =>
        {
            lock (_fileLock)
            {
                if (!File.Exists(_csvFilePath))
                {
                    return new List<ModelTraining>();
                }

                string[] lines = File.ReadAllLines(_csvFilePath);
                return lines.Skip(1) // Skip header
                    .Select(CsvHelper.ConvertCsvLineToEntity)
                    .Where(e => e.TrainingStatus == trainingStatus)
                    .ToList();
            }
        });
    }

    public async Task UpdateAsync(ModelTraining entity)
    {
        await Task.Run(() =>
        {
            lock (_fileLock)
            {
                if (!File.Exists(_csvFilePath))
                {
                    throw new FileNotFoundException("CSV file not found");
                }

                var lines = File.ReadAllLines(_csvFilePath).ToList();
                var found = false;

                for (var i = 1; i < lines.Count; i++) // Start from 1 to skip header
                {
                    var currentEntity = CsvHelper.ConvertCsvLineToEntity(lines[i]);
                    if (currentEntity.Id == entity.Id)
                    {
                        lines[i] = CsvHelper.ConvertEntityToCsvLine(entity);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    throw new KeyNotFoundException($"Entity with ID {entity.Id} not found");
                }

                File.WriteAllLines(_csvFilePath, lines);
            }
        });
    }
}
