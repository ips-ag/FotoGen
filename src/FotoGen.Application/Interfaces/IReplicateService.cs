using FotoGen.Common;
using FotoGen.Common.Contracts.Replicated.CreateModel;
using FotoGen.Common.Contracts.Replicated.GetTrainedModelStatus;
using FotoGen.Common.Contracts.Replicated.TrainModel;
using FotoGen.Common.Contracts.Replicated.UseModel;

namespace FotoGen.Application.Interfaces
{
    public interface IReplicateService 
    {
        public Task<BaseResponse<bool>> GetModelAsync(string name);
        public Task<BaseResponse<bool>> CreateReplicateModelAsync(CreateReplicateModelRequestDto request);
        public Task<BaseResponse<TrainModelResponseDto>> TrainModelAsync(TrainModelRequestDto request);
        public Task<BaseResponse<GetTrainedModelStatusResponseDto>> GetTrainModelStatusAsync(string trainModelId);
        public Task<BaseResponse<UseModelResponseDto>> GeneratePhotoAsync(string prompt, string modelName);
    }
}
