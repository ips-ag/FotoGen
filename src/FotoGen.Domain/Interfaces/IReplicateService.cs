using FotoGen.Common;
using FotoGen.Domain.Entities;

namespace FotoGen.Domain.Interfaces
{
    public interface IReplicateService 
    {
        Task<BaseResponse<ReplicateModelEntity>> CreateTrainModelAsync(string owner, string name);
        Task<BaseResponse<TrainModelEntity>> TrainModelAsync(string destination, string imageUrl, string triggerWords);
    }
}
