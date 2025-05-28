using FotoGen.Domain.Entities.Response;
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
                nameof(ErrorCode.Validation) => new BadRequestObjectResult(response),
                nameof(ErrorCode.UnauthorizedAccess) => new UnauthorizedObjectResult(response),
                nameof(ErrorCode.Forbidden) => new ObjectResult(response) { StatusCode = StatusCodes.Status403Forbidden },
                _ => new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError }
            };
        }
    }
}
