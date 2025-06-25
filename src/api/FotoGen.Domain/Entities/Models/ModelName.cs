using System.Text.RegularExpressions;
using FotoGen.Domain.Entities.Requests;

namespace FotoGen.Domain.Entities.Models;

public class ModelName
{
    public string Value { get; }

    public ModelName(User user)
    {
        Value = FormatToValidModelName($"{user.FullName}_{user.Id}");

    }

    public static implicit operator string(ModelName modelName)
    {
        return modelName.Value;
    }
    private static string FormatToValidModelName(string input)
    {
        input = input.ToLower();
        input = Regex.Replace(input, @"[^a-z0-9._-]", "");
        input = Regex.Replace(input, @"^[-_.]+|[-_.]+$", "");
        input = Regex.Replace(input, "(_-|-_)+", "");
        return input;
    }
}
