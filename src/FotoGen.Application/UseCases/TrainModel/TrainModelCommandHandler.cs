using FotoGen.Application.Interfaces;
using FotoGen.Common;
using FotoGen.Common.Contracts.Replicated.CreateModel;
using FotoGen.Common.Contracts.Replicated.TrainModel;
using FotoGen.Domain.Entities;
using FotoGen.Domain.Interfaces;
using FotoGen.Domain.ValueObjects;
using MediatR;

namespace FotoGen.Application.UseCases.TrainModel
{
    public class TrainModelCommandHandler : IRequestHandler<TrainModelCommand, BaseResponse<TrainModelResponse>>
    {
        private readonly IReplicateModelRepository _modelRepository;
        private readonly ITrainedModelRepository _trainModelEntity;
        private readonly IReplicateService _replicateService;
        public async Task<BaseResponse<TrainModelResponse>> Handle(TrainModelCommand request, CancellationToken cancellationToken)
        {
            var model = await _modelRepository.GetByNameAsync(request.Name);
            if (model == null) 
            {
                var createReplicateModelRequestDto = new CreateReplicateModelRequestDto { Name = request.Name, Description = request.Description };
                var createModelResult = await _replicateService.CreateReplicateModelAsync(createReplicateModelRequestDto);
                if (!createModelResult.IsSuccess) return BaseResponse<TrainModelResponse>.Fail(ErrorCode.CreateReplicateModelFail);
                model = new ReplicateModelEntity(request.Name, request.Description);
                await _modelRepository.AddAsync(model);
            }
            var trainModelDto = new TrainModelRequestDto { Name = request.Name, ImageUrl = request.InputImageUrl, TriggerWords = request.TriggerWords };
            var trainModelResult = await _replicateService.TrainModelAsync(trainModelDto);
            if (!trainModelResult.IsSuccess) return BaseResponse<TrainModelResponse>.Fail(ErrorCode.TrainReplicateModelFail);
            var result = trainModelResult.Data;
            Enum.TryParse<TrainModelStatus>(result?.Status, ignoreCase: true, out var statusEnum);
            var trainModelEntity = new TrainModelEntity(result.Id, model.Id.ToString(), request.InputImageUrl, request.TriggerWords, statusEnum, result.CanceledUrl);
            await _trainModelEntity.AddAsync(trainModelEntity);
            var response = new TrainModelResponse { Id = trainModelEntity.Id, CanceledUrl = trainModelEntity?.CanceledUrl, Status = trainModelEntity.Status.ToString() };
            return BaseResponse<TrainModelResponse>.Success(response);
        }
    }
}
