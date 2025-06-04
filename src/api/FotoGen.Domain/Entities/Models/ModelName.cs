using FotoGen.Domain.Entities.Requests;

namespace FotoGen.Domain.Entities.Models;

public class ModelName
{
    public string Value { get; }

    public ModelName(User user)
    {
        string namePart = Normalize(user.FullName);
        string idPart = Normalize(user.Id);
        Value = $"{namePart}_{idPart}";
    }

    public static implicit operator string(ModelName modelName)
    {
        return modelName.Value;
    }

    private static string Normalize(string value)
    {
        return value.Replace(" ", "_").ToLower();
    }
}
