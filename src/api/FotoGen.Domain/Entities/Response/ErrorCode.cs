namespace FotoGen.Domain.Entities.Response;

public enum ErrorCode
{
    UnauthorizedAccess,
    Validation,
    Forbidden,
    CreateReplicateModelFail,
    GeneratePhotoFail,
    GetReplicateTrainModelFail,
    GetReplicateModelFail,
    TrainReplicateModelFail,
    ImageGenerationResponseEmpty
}
