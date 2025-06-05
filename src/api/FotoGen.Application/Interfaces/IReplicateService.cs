using FotoGen.Domain.Entities.Images;
using FotoGen.Domain.Entities.Models;
using FotoGen.Domain.Entities.Response;

namespace FotoGen.Application.Interfaces;

public interface IReplicateService
{
    public ValueTask<TrainedModel?> GetTrainedModelByNameAsync(string name, CancellationToken cancel);
    public Task<BaseResponse<bool>> CreateTrainedModelAsync(CreateTrainedModelRequest request);
    public Task<BaseResponse<TrainModelResponse>> CreateModelTrainingAsync(TrainModelRequest request);
    public Task<BaseResponse<QueryModelTrainingStatus>> GetModelTrainingStatusAsync(string trainModelId);
    public Task<BaseResponse<GenerateImageResponse>> GeneratePhotoAsync(string prompt, TrainedModel model, CancellationToken cancel);
}
