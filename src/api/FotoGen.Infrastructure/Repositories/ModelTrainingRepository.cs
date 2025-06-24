using FotoGen.Domain.Entities.Models;
using FotoGen.Domain.Repositories;
using FotoGen.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace FotoGen.Infrastructure.Repositories;

public class ModelTrainingRepository : IModelTrainingRepository
{
    private readonly IOptionsMonitor<ModelTrainingSettings> _options;
    private readonly SemaphoreSlim _fileLock = new(1, 1);

    public ModelTrainingRepository(IOptionsMonitor<ModelTrainingSettings> options)
    {
        _options = options;
        _options.OnChange(Initialize);
    }

    public async Task CreateAsync(ModelTraining entity, CancellationToken cancel)
    {
        string line = CsvHelper.ConvertEntityToCsvLine(entity);
        await _fileLock.WaitAsync(cancel);
        try
        {
            await File.AppendAllTextAsync(_options.CurrentValue.CsvFilePath, line + Environment.NewLine, cancel);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<List<ModelTraining>> GetByStatusAsync(
        ModelTrainingStatus trainingStatus,
        CancellationToken cancel)
    {
        await _fileLock.WaitAsync(cancel);
        try
        {
            if (!File.Exists(_options.CurrentValue.CsvFilePath)) return [];
            string[] lines = await File.ReadAllLinesAsync(_options.CurrentValue.CsvFilePath, cancel);
            return lines.Skip(1) // Skip header
                .Select(CsvHelper.ConvertCsvLineToEntity)
                .Where(e => e.TrainingStatus == trainingStatus)
                .ToList();
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task UpdateAsync(ModelTraining entity, CancellationToken cancel)
    {
        await _fileLock.WaitAsync(cancel);
        try
        {
            if (!File.Exists(_options.CurrentValue.CsvFilePath)) throw new FileNotFoundException("CSV file not found");
            string[] lines = await File.ReadAllLinesAsync(_options.CurrentValue.CsvFilePath, cancel);
            var found = false;
            for (var i = 1; i < lines.Length; i++) // Start from 1 to skip header
            {
                var currentEntity = CsvHelper.ConvertCsvLineToEntity(lines[i]);
                if (currentEntity.Id != entity.Id) continue;
                lines[i] = CsvHelper.ConvertEntityToCsvLine(entity);
                found = true;
                break;
            }
            if (!found) throw new KeyNotFoundException($"Entity with ID {entity.Id} not found");
            File.WriteAllLines(_options.CurrentValue.CsvFilePath, lines);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private static void Initialize(ModelTrainingSettings settings)
    {
        string csvFilePath = settings.CsvFilePath;
        string? directory = Path.GetDirectoryName(csvFilePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        if (!File.Exists(csvFilePath))
        {
            File.WriteAllLines(
                csvFilePath,
                ["Id,ModelName,UserEmail,UserName,ImageUrl,TriggerWord,Status,CreatedAt,SuccessedAt,CanceledUrl"]);
        }
    }
}
