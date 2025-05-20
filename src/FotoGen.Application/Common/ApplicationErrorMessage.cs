using Microsoft.AspNetCore.Http;

namespace FotoGen.Application.Common
{
    public class ApplicationErrorMessage
    {
        private static readonly Dictionary<ApplicationErrorCode, ErrorInfo> _errors = new()
        {
            { ApplicationErrorCode.UnauthorizedAccess, new ErrorInfo { Message = "You do not have access to this resource.", StatusCode = StatusCodes.Status403Forbidden } },
        };

        public static ErrorInfo Get(ApplicationErrorCode code) =>
            _errors.TryGetValue(code, out var error)
                ? error
                : new ErrorInfo { Message = "Unknown error.", StatusCode = StatusCodes.Status500InternalServerError };
    }
}
