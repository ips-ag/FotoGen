using FotoGen.Common;
using FotoGen.Common.Contracts.Replicated.CreateModel;

namespace FotoGen.Application.Interfaces
{
    public interface IReplicateService 
    {
        public Task<BaseResponse<CreateReplicateModelResultDto>> CreateTrainModel(CreateReplicateModelRequestDto request);
    }
}
