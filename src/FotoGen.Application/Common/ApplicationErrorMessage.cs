using Microsoft.AspNetCore.Http;

namespace FotoGen.Application.Common
{
    public class ApplicationErrorMessage
    {
        private static readonly Dictionary<ApplicationErrorCode, string> _errors = new()
        {
            { ApplicationErrorCode.UnauthorizedAccess, "You do not have access to this resource." },
            { ApplicationErrorCode.Validation, "Validation failed." },
        };

        public static string Get(ApplicationErrorCode code) =>
            _errors.TryGetValue(code, out var error)
                ? error
                : "Unknown error.";
    }
}
