using FotoGen.Domain.Entities.Images;
using FotoGen.Domain.Entities.Models;
using FotoGen.Domain.Entities.Response;

namespace FotoGen.Application.Interfaces;

public interface IReplicateService
{
    public ValueTask<TrainedModel?> GetTrainedModelByNameAsync(string name, CancellationToken cancel);

    public Task<BaseResponse<bool>> CreateTrainedModelAsync(
        CreateTrainedModelRequest request,
        CancellationToken cancel);

    public Task<BaseResponse<TrainModelResponse>> CreateModelTrainingAsync(
        TrainModelRequest request,
        CancellationToken cancel);

    public Task<BaseResponse<QueryModelTrainingStatus>> GetModelTrainingStatusAsync(
        string trainModelId,
        CancellationToken cancel);

    public Task<BaseResponse<GenerateImageResponse>> GeneratePhotoAsync(
        string prompt,
        TrainedModel model,
        CancellationToken cancel);
}
