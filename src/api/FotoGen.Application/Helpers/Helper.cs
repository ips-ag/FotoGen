using FotoGen.Domain.Entities.Requests;

namespace FotoGen.Application.Helpers
{
    public static class Helper
    {
        public static string GetModelNameFromUserInfo(User user) 
        {
            var modelName = $"{user.GivenName}_{user.FamilyName}_{user.Id}";
            return modelName.Replace(" ", "").ToLower();
        }
        public static string GetTriggerWordFromModelName (string modelName)
        {
            return char.ToUpperInvariant(modelName.Split('_')[0][0]) + modelName.Split('_')[0][1..];
        }

    }
}
