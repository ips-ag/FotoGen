using System.Globalization;
using FotoGen.Domain.Entities;
using FotoGen.Domain.ValueObjects;

namespace FotoGen.Infrastructure
{
    public static class CsvHelper
    {
        public static string EscapeCsvField(string field)
        {
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
            {
                return $"\"{field.Replace("\"", "\"\"")}\"";
            }
            return field;
        }

        public static string UnescapeCsvField(string field)
        {
            if (field.StartsWith("\"") && field.EndsWith("\""))
            {
                return field.Substring(1, field.Length - 2).Replace("\"\"", "\"");
            }
            return field;
        }

        public static IEnumerable<string> ParseCsvLine(string line)
        {
            bool inQuotes = false;
            int start = 0;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (line[i] == ',' && !inQuotes)
                {
                    yield return line.Substring(start, i - start);
                    start = i + 1;
                }
            }

            yield return line.Substring(start);
        }

        public static string ConvertEntityToCsvLine(TrainModelEntity entity)
        {
            return $"{EscapeCsvField(entity.Id)}," +
                   $"{EscapeCsvField(entity.ModelName)}," +
                   $"{EscapeCsvField(entity.UserEmail)}," +
                   $"{EscapeCsvField(entity.ImageUrl)}," +
                   $"{EscapeCsvField(entity.TriggerWord)}," +
                   $"{entity.Status}," +
                   $"{entity.CreatedAt.ToString("o", CultureInfo.InvariantCulture)}," +
                   $"{(entity.SuccessedAt.HasValue ? entity.SuccessedAt.Value.ToString("o", CultureInfo.InvariantCulture) : "")}," +
                   $"{EscapeCsvField(entity.CanceledUrl ?? "")}";
        }

        public static TrainModelEntity ConvertCsvLineToEntity(string csvLine)
        {
            var parts = ParseCsvLine(csvLine).ToArray();

            return new TrainModelEntity
            {
                Id = UnescapeCsvField(parts[0]),
                ModelName = UnescapeCsvField(parts[1]),
                UserEmail = UnescapeCsvField(parts[2]),
                ImageUrl = UnescapeCsvField(parts[3]),
                TriggerWord = UnescapeCsvField(parts[4]),
                Status = (TrainModelStatus)Enum.Parse(typeof(TrainModelStatus), parts[5]),
                CreatedAt = DateTime.Parse(parts[6], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                SuccessedAt = string.IsNullOrEmpty(parts[7]) ? null : DateTime.Parse(parts[7], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                CanceledUrl = string.IsNullOrEmpty(parts[8]) ? null : UnescapeCsvField(parts[8])
            };
        }
    }
}
