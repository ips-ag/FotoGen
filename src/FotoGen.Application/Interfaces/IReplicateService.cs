using FotoGen.Common;
using FotoGen.Common.Contracts.Replicated.CreateModel;
using FotoGen.Common.Contracts.Replicated.TrainModel;

namespace FotoGen.Application.Interfaces
{
    public interface IReplicateService 
    {
        public Task<BaseResponse<bool>> CreateReplicateModelAsync(CreateReplicateModelRequestDto request);
        public Task<BaseResponse<TrainModelResponseDto>> TrainModelAsync(TrainModelRequestDto request);
    }
}
