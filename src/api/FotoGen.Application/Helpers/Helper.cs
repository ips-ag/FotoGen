namespace FotoGen.Application.Helpers
{
    public static class Helper
    {
        public static string GetTriggerWordFromUserName(string userName)
        {
            return userName.First().ToString();
        }

    }
}
