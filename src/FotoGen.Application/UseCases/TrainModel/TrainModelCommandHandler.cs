using FotoGen.Application.Helpers;
using FotoGen.Application.Interfaces;
using FotoGen.Domain.Entities;
using FotoGen.Domain.Entities.Models;
using FotoGen.Domain.Entities.Response;
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
                var createReplicateModelRequestDto = new CreateModelRequest(request.UserName);
                var createModelResult = await _replicateService.CreateReplicateModelAsync(createReplicateModelRequestDto);
                if (!createModelResult.IsSuccess) return BaseResponse<TrainModelResponse>.Fail(ErrorCode.CreateReplicateModelFail);
            }
            var triggerWord = Helper.GetTriggerWordFromUserName(request.ModelName);
            var trainModelDto = new TrainModelRequest(request.ModelName, request.InputImageUrl, triggerWord);
            var trainModelResult = await _replicateService.TrainModelAsync(trainModelDto);
            ////test
            //var trainModelResponseDto = new TrainModelResponseDto { CanceledUrl = "https://api.replicate.com/v1/predictions/vytn2aq645rme0cq2q6az1erym/cancel", Id = "vytn2aq645rme0cq2q6az1erym", Status = "staring" };
            //var trainModelResult = BaseResponse<TrainModelResponseDto>.Success(trainModelResponseDto);
            ////end test code
            if (!trainModelResult.IsSuccess) return BaseResponse<TrainModelResponse>.Fail(ErrorCode.TrainReplicateModelFail);
            var trainModelEntity = new TrainModelEntity(trainModelResult.Data.Id, request.ModelName, request.UserEmail, request.InputImageUrl, triggerWord, TrainModelStatus.Starting, trainModelResult.Data.CancelUrl);
            await _trainedModelRepository.AddAsync(trainModelEntity);
            var result = new TrainModelResponse { Id = trainModelResult.Data.Id, Status = trainModelResult.Data.Status, CanceledUrl = trainModelResult.Data.CancelUrl };
            return BaseResponse<TrainModelResponse>.Success(result);
        }
    }
}
