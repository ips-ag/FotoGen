using FotoGen.Domain.Entities.Response;
using Microsoft.AspNetCore.Mvc;

namespace FotoGen.Extensions;

public static class BaseResponseExtensions
{
    public static IActionResult ToActionResult<T>(this BaseResponse<T> response)
    {
        if (response.IsSuccess) return new OkObjectResult(response);

        return response.ErrorCode switch
        {
            ErrorCode.Validation => new BadRequestObjectResult(response),
            ErrorCode.UnauthorizedAccess => new UnauthorizedObjectResult(response),
            ErrorCode.Forbidden => new ObjectResult(response) { StatusCode = StatusCodes.Status403Forbidden },

            //Not found
            ErrorCode.ReplicateModelNotFound => new ObjectResult(response) { StatusCode = StatusCodes.Status404NotFound },

            // Replicate-related errors (treated as 500 Internal Server Error)
            ErrorCode.CreateReplicateModelFail => new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError },
            ErrorCode.GeneratePhotoFail => new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError },
            ErrorCode.GetReplicateTrainModelFail => new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError },
            ErrorCode.GetReplicateModelFail => new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError },
            ErrorCode.TrainReplicateModelFail => new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError },
            ErrorCode.ImageGenerationResponseEmpty => new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError },

            _ => new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError }
        };
    }
}
