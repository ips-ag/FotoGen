using System.Text.RegularExpressions;
using FotoGen.Domain.Entities.Requests;

namespace FotoGen.Domain.Entities.Models;

public partial class ModelName
{
    public string Value { get; }

    public ModelName(User user)
    {
        Value = Normalize($"{user.FullName}_{user.Id}");
    }

    public static implicit operator string(ModelName modelName)
    {
        return modelName.Value;
    }

    private static string Normalize(string input)
    {
        input = input.Replace(" ", "_").ToLowerInvariant();
        input = ValidCharacters().Replace(input, "");
        input = LeadingAndTrailingSeparators().Replace(input, "");
        input = ForbiddenCharacterCombinations().Replace(input, "");
        return input;
    }

    [GeneratedRegex(@"[^a-z0-9._-]")]
    private static partial Regex ValidCharacters();

    [GeneratedRegex(@"^[-_.]+|[-_.]+$")]
    private static partial Regex LeadingAndTrailingSeparators();

    [GeneratedRegex("(_-|-_|--)+")]
    private static partial Regex ForbiddenCharacterCombinations();
}
