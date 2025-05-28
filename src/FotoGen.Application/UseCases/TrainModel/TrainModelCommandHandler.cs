using FotoGen.Application.Helpers;
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
        private readonly IReplicateService _replicateService;
        private readonly ITrainedModelRepository _trainedModelRepository;
        public TrainModelCommandHandler(IReplicateService replicateService, ITrainedModelRepository trainedModelRepository)
        {
            _replicateService = replicateService;
            _trainedModelRepository = trainedModelRepository; 
        }
        public async Task<BaseResponse<TrainModelResponse>> Handle(TrainModelCommand request, CancellationToken cancellationToken)
        {
            var isModelExisted = await _replicateService.GetModelAsync(request.ModelName);
            if (!isModelExisted.IsSuccess)
            {
                var createReplicateModelRequestDto = new CreateReplicateModelRequestDto { Name = request.ModelName };
                var createModelResult = await _replicateService.CreateReplicateModelAsync(createReplicateModelRequestDto);
                if (!createModelResult.IsSuccess) return BaseResponse<TrainModelResponse>.Fail(ErrorCode.CreateReplicateModelFail);
            }
            var triggerWord = Helper.GetTriggerWordFromUserName(request.ModelName);
            var trainModelDto = new TrainModelRequestDto { Name = request.ModelName, ImageUrl = request.InputImageUrl, TriggerWord = triggerWord };
            //var trainModelResult = await _replicateService.TrainModelAsync(trainModelDto);
            //test
            var trainModelResponseDto = new TrainModelResponseDto { CanceledUrl = "https://api.replicate.com/v1/predictions/vytn2aq645rme0cq2q6az1erym/cancel", Id = "vytn2aq645rme0cq2q6az1erym", Status = "staring" };
            var trainModelResult = BaseResponse<TrainModelResponseDto>.Success(trainModelResponseDto);
            //end test code
            if (!trainModelResult.IsSuccess) return BaseResponse<TrainModelResponse>.Fail(ErrorCode.TrainReplicateModelFail);
            var trainModelEntity = new TrainModelEntity(trainModelResult.Data.Id, request.ModelName, request.UserEmail, request.InputImageUrl, triggerWord, TrainModelStatus.Starting, trainModelResult.Data.CanceledUrl);
            await _trainedModelRepository.AddAsync(trainModelEntity);
            var result = new TrainModelResponse { Id = trainModelResult.Data.Id, Status = trainModelResult.Data.Status, CanceledUrl = trainModelResult.Data.CanceledUrl };
            return BaseResponse<TrainModelResponse>.Success(result);
        }
    }
}
