using FotoGen.Domain.Entities.Images;
using FotoGen.Domain.Entities.Models;
using FotoGen.Domain.Entities.Response;

namespace FotoGen.Application.Interfaces;

public interface IReplicateService
{
    public Task<BaseResponse<bool>> GetModelAsync(string name);
    public Task<BaseResponse<bool>> CreateReplicateModelAsync(CreateModelRequest request);
    public Task<BaseResponse<TrainModelResponse>> TrainModelAsync(TrainModelRequest request);
    public Task<BaseResponse<QueryModelTrainingStatus>> GetTrainModelStatusAsync(string trainModelId);
    public Task<BaseResponse<GenerateImageResponse>> GeneratePhotoAsync(string prompt, string modelName);
}
