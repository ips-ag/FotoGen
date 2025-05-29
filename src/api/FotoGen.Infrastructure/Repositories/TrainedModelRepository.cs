using FotoGen.Domain.Entities;
using FotoGen.Domain.Interfaces;
using FotoGen.Domain.ValueObjects;

namespace FotoGen.Infrastructure.Repositories
{
    public class TrainedModelRepository : ITrainedModelRepository
    {
        private readonly string _csvFilePath;
        private readonly object _fileLock = new object();

        public TrainedModelRepository(string csvFilePath)
        {
            _csvFilePath = csvFilePath;

            // Ensure directory exists
            var directory = Path.GetDirectoryName(_csvFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create file with headers if it doesn't exist
            if (!File.Exists(_csvFilePath))
            {
                File.WriteAllText(_csvFilePath,
                    "Id,ModelName,UserEmail,ImageUrl,TriggerWord,Status,CreatedAt,SuccessedAt,CanceledUrl\n");
            }
        }

        public async Task AddAsync(TrainModelEntity entity)
        {
            var line = CsvHelper.ConvertEntityToCsvLine(entity);

            await Task.Run(() =>
            {
                lock (_fileLock)
                {
                    File.AppendAllText(_csvFilePath, line + Environment.NewLine);
                }
            });
        }

        public async Task<List<TrainModelEntity>> GetByStatusAsync(TrainModelStatus status)
        {
            return await Task.Run(() =>
            {
                lock (_fileLock)
                {
                    if (!File.Exists(_csvFilePath))
                        return new List<TrainModelEntity>();

                    var lines = File.ReadAllLines(_csvFilePath);
                    return lines.Skip(1) // Skip header
                        .Select(CsvHelper.ConvertCsvLineToEntity)
                        .Where(e => e.Status == status)
                        .ToList();
                }
            });
        }

        public async Task UpdateAsync(TrainModelEntity entity)
        {
            await Task.Run(() =>
            {
                lock (_fileLock)
                {
                    if (!File.Exists(_csvFilePath))
                        throw new FileNotFoundException("CSV file not found");

                    var lines = File.ReadAllLines(_csvFilePath).ToList();
                    bool found = false;

                    for (int i = 1; i < lines.Count; i++) // Start from 1 to skip header
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
                        throw new KeyNotFoundException($"Entity with ID {entity.Id} not found");

                    File.WriteAllLines(_csvFilePath, lines);
                }
            });
        }
    }
}
