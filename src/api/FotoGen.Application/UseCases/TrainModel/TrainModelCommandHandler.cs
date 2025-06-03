using FluentValidation;
using FotoGen.Application.Helpers;
using FotoGen.Application.Interfaces;
using FotoGen.Domain.Entities;
using FotoGen.Domain.Entities.Models;
using FotoGen.Domain.Entities.Response;
using FotoGen.Domain.Interfaces;
using FotoGen.Domain.ValueObjects;
using MediatR;

namespace FotoGen.Application.UseCases.TrainModel;

public class TrainModelCommandHandler : IRequestHandler<TrainModelCommand, BaseResponse<TrainModelResponse>>
{
    private readonly IReplicateService _replicateService;
    private readonly ITrainedModelRepository _trainedModelRepository;
    private readonly IValidator<TrainModelCommand> _validator;

    public TrainModelCommandHandler(
        IReplicateService replicateService, 
        ITrainedModelRepository trainedModelRepository, 
        IValidator<TrainModelCommand> validator)
    {
        _replicateService = replicateService;
        _trainedModelRepository = trainedModelRepository;
        _validator = validator;
    }

    public async Task<BaseResponse<TrainModelResponse>> Handle(
        TrainModelCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BaseResponse<TrainModelResponse>.Fail(validationResult.ToDictionary());
        }
        var isModelExisted = await _replicateService.GetModelAsync(request.ModelName);
        if (!isModelExisted.IsSuccess && isModelExisted.ErrorCode == ErrorCode.ReplicateModelNotFound)
        {
            var createReplicateModelRequestDto = new CreateModelRequest(request.ModelName);
            var createModelResult = await _replicateService.CreateReplicateModelAsync(createReplicateModelRequestDto);
            if (!createModelResult.IsSuccess)
            {
                return BaseResponse<TrainModelResponse>.Fail(ErrorCode.CreateReplicateModelFail);
            }
        }
        string triggerWord = Helper.GetTriggerWordFromUserName(request.ModelName);
        var trainModelDto = new TrainModelRequest(request.ModelName, request.InputImageUrl, triggerWord);
        var trainModelResult = await _replicateService.TrainModelAsync(trainModelDto);
        ////test
        //var trainModelResponseDto = new TrainModelResponseDto { CanceledUrl = "https://api.replicate.com/v1/predictions/vytn2aq645rme0cq2q6az1erym/cancel", Id = "vytn2aq645rme0cq2q6az1erym", Status = "staring" };
        //var trainModelResult = BaseResponse<TrainModelResponseDto>.Success(trainModelResponseDto);
        ////end test code
        if (!trainModelResult.IsSuccess)
        {
            return BaseResponse<TrainModelResponse>.Fail(ErrorCode.TrainReplicateModelFail);
        }
        var trainModelEntity = new TrainModelEntity(
            trainModelResult.Data.Id,
            request.ModelName,
            request.UserEmail,
            request.InputImageUrl,
            triggerWord,
            TrainModelStatus.InProgress,
            trainModelResult.Data.CancelUrl);
        await _trainedModelRepository.AddAsync(trainModelEntity);
        var result = new TrainModelResponse(
            trainModelResult.Data.Id,
            trainModelResult.Data.Status,
            trainModelResult.Data.CancelUrl);
        return BaseResponse<TrainModelResponse>.Success(result);
    }
}
