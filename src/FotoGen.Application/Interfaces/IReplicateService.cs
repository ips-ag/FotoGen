using FotoGen.Common;
using FotoGen.Common.Contracts.Replicated.CreateModel;
using FotoGen.Common.Contracts.Replicated.GetTrainedModelStatus;
using FotoGen.Common.Contracts.Replicated.TrainModel;
using FotoGen.Domain.ValueObjects;

namespace FotoGen.Application.Interfaces
{
    public interface IReplicateService 
    {
        public Task<BaseResponse<bool>> CreateReplicateModelAsync(CreateReplicateModelRequestDto request);
        public Task<BaseResponse<TrainModelResponseDto>> TrainModelAsync(TrainModelRequestDto request);
        public Task<BaseResponse<GetTrainedModelStatusResponseDto>> GetTrainModelStatusAsync(string trainModelId);
    }
}
