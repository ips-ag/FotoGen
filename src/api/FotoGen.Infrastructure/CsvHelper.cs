using System.Globalization;
using FotoGen.Domain.Entities;
using FotoGen.Domain.Entities.Models;

namespace FotoGen.Infrastructure;

public static class CsvHelper
{
    public static string ConvertEntityToCsvLine(ModelTraining entity)
    {
        return $"{EscapeCsvField(entity.Id)}," +
            $"{EscapeCsvField(entity.ModelName)}," +
            $"{EscapeCsvField(entity.UserEmail)}," +
            $"{EscapeCsvField(entity.UserName)}," +
            $"{EscapeCsvField(entity.ImageUrl)}," +
            $"{EscapeCsvField(entity.TriggerWord)}," +
            $"{entity.TrainingStatus}," +
            $"{entity.CreatedAt.ToString("o", CultureInfo.InvariantCulture)}," +
            $"{(entity.SucceededAt.HasValue ? entity.SucceededAt.Value.ToString("o", CultureInfo.InvariantCulture) : "")}," +
            $"{EscapeCsvField(entity.CanceledUrl ?? "")}";
    }

    public static ModelTraining ConvertCsvLineToEntity(string csvLine)
    {
        string[] parts = ParseCsvLine(csvLine).ToArray();
        return new ModelTraining(
            Id: UnescapeCsvField(parts[0]),
            ModelName: UnescapeCsvField(parts[1]),
            UserEmail: UnescapeCsvField(parts[2]),
            UserName: UnescapeCsvField(parts[3]),
            ImageUrl: UnescapeCsvField(parts[4]),
            TriggerWord: UnescapeCsvField(parts[5]),
            TrainingStatus: (ModelTrainingStatus)Enum.Parse(typeof(ModelTrainingStatus), parts[6]),
            CreatedAt: DateTime.Parse(parts[7], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
            SucceededAt: string.IsNullOrEmpty(parts[8])
                ? null
                : DateTime.Parse(parts[7], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
            CanceledUrl: string.IsNullOrEmpty(parts[9]) ? null : UnescapeCsvField(parts[8])
        );
    }

    private static string EscapeCsvField(string field)
    {
        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }
        return field;
    }

    private static string UnescapeCsvField(string field)
    {
        if (field.StartsWith("\"") && field.EndsWith("\""))
        {
            return field.Substring(1, field.Length - 2).Replace("\"\"", "\"");
        }
        return field;
    }

    private static IEnumerable<string> ParseCsvLine(string line)
    {
        var inQuotes = false;
        var start = 0;

        for (var i = 0; i < line.Length; i++)
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
}
