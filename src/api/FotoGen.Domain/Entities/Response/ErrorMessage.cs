namespace FotoGen.Domain.Entities.Response;

public class ErrorMessage
{
    private static readonly Dictionary<ErrorCode, string> _errors = new()
{
    { ErrorCode.UnauthorizedAccess, "You do not have access to this resource." },
    { ErrorCode.Validation, "Validation failed." },
    { ErrorCode.Forbidden, "Access to this resource is forbidden." },
    { ErrorCode.CreateReplicateModelFail, "Failed to create Replicate model." },
    { ErrorCode.GeneratePhotoFail, "Failed to generate photo." },
    { ErrorCode.GetReplicateTrainModelFail, "Failed to retrieve Replicate training model." },
    { ErrorCode.GetReplicateModelFail, "Failed to retrieve Replicate model." },
    { ErrorCode.TrainReplicateModelFail, "Failed to train Replicate model." },
    { ErrorCode.ImageGenerationResponseEmpty, "Image generation returned an empty response." }
};

    public static string Get(ErrorCode code)
    {
        return _errors.GetValueOrDefault(code, "Unknown error.");
    }
}
