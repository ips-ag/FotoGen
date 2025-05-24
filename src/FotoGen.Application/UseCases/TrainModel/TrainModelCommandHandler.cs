using FotoGen.Common;
using FotoGen.Domain.Entities;
using FotoGen.Domain.Interfaces;
using MediatR;

namespace FotoGen.Application.UseCases.TrainModel
{
    public class TrainModelCommandHandler : IRequestHandler<TrainModelCommand, BaseResponse<TrainModelResponse>>
    {
        private readonly IModelRepository<ReplicateModelEntity> _modelRepository;
        private readonly IModelRepository<TrainModelEntity> _trainModelEntity;
        private readonly IReplicateService _replicateService;
        public async Task<BaseResponse<TrainModelResponse>> Handle(TrainModelCommand request, CancellationToken cancellationToken)
        {
            var destination = $"{request.Owner}/{request.Name}";
            var model = await _modelRepository.GetByDestinationAsync(destination);
            if (model == null) 
            {
                var createModelResult = await _replicateService.CreateTrainModelAsync(request.Owner, request.Name);
                if (!createModelResult.IsSuccess) return BaseResponse<TrainModelResponse>.Fail(ErrorCode.CreateReplicateModelFail);
                await _modelRepository.AddAsync(createModelResult.Data);
            }
            var trainModelResult = await _replicateService.TrainModelAsync(destination, request.InputImageUrl, request.TriggerWords);
            if (!trainModelResult.IsSuccess) return BaseResponse<TrainModelResponse>.Fail(ErrorCode.TrainReplicateModelFail);
            await _trainModelEntity.AddAsync(trainModelResult.Data);
        }
    }
}
