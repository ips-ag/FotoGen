using FotoGen.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace FotoGen.Externsions
{
    public static class BaseResponseExtensions
    {
        public static IActionResult ToActionResult<T>(this BaseResponse<T> response)
        {
            if (response.IsSuccess)
                return new OkObjectResult(response);

            return response.ErrorCode switch
            {
                nameof(ApplicationErrorCode.Validation) => new BadRequestObjectResult(response),
                nameof(ApplicationErrorCode.UnauthorizedAccess) => new UnauthorizedObjectResult(response),
                nameof(ApplicationErrorCode.Forbidden) => new ObjectResult(response) { StatusCode = StatusCodes.Status403Forbidden },
                _ => new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError }
            };
        }
    }
}
