using Microsoft.AspNetCore.Http;

namespace FotoGen.Common
{
    public class ErrorMessage
    {
        private static readonly Dictionary<ErrorCode, string> _errors = new()
        {
            { ErrorCode.UnauthorizedAccess, "You do not have access to this resource." },
            { ErrorCode.Validation, "Validation failed." },
            { ErrorCode.CreateReplicateModelFail, "Failed to create Replicate model." }
        };

        public static string Get(ErrorCode code) =>
            _errors.TryGetValue(code, out var error)
                ? error
                : "Unknown error.";
    }
}
