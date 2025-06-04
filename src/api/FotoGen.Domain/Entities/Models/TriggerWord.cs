using FotoGen.Domain.Entities.Requests;

namespace FotoGen.Domain.Entities.Models;

public class TriggerWord
{
    public string Value { get; }

    public TriggerWord(User user)
    {
        Value = Normalize(user.FirstName);
    }

    public static implicit operator string(TriggerWord triggerWord)
    {
        return triggerWord.Value;
    }

    private static string Normalize(string value)
    {
        return value.Replace(" ", "_").ToUpperInvariant();
    }
}
