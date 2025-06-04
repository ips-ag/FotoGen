using FluentValidation;
using FotoGen.Application.Helpers;
using FotoGen.Application.Interfaces;
using FotoGen.Domain.Entities.Models;
using FotoGen.Domain.Entities.Response;
using FotoGen.Domain.Repositories;
using FotoGen.Domain.ValueObjects;
using MediatR;

namespace FotoGen.Application.UseCases.TrainModel;

public class TrainModelCommandHandler : IRequestHandler<TrainModelCommand, BaseResponse<TrainModelResponse>>
{
    private readonly IReplicateService _replicateService;
    private readonly IModelTrainingRepository _modelTrainingRepository;
    private readonly IRequestContextRepository _requestContextRepository;
    private readonly IValidator<TrainModelCommand> _validator;

    public TrainModelCommandHandler(
        IReplicateService replicateService,
        IModelTrainingRepository modelTrainingRepository,
        IValidator<TrainModelCommand> validator,
        IRequestContextRepository requestContextRepository)
    {
        _replicateService = replicateService;
        _modelTrainingRepository = modelTrainingRepository;
        _validator = validator;
        _requestContextRepository = requestContextRepository;
    }

    public async Task<BaseResponse<TrainModelResponse>> Handle(
        TrainModelCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BaseResponse<TrainModelResponse>.Fail(validationResult.ToDictionary());
        }
        var user = (await _requestContextRepository.GetAsync()).User;
        string modelName = user.Id;
        var modelExists = await _replicateService.GetModelAsync(modelName);
        if (modelExists is { IsSuccess: false, ErrorCode: ErrorCode.ReplicateModelNotFound })
        {
            var createReplicateModelRequestDto = new CreateModelRequest(modelName);
            var createModelResult = await _replicateService.CreateReplicateModelAsync(createReplicateModelRequestDto);
            if (!createModelResult.IsSuccess)
            {
                return BaseResponse<TrainModelResponse>.Fail(ErrorCode.CreateReplicateModelFail);
            }
        }
        string triggerWord = Helper.GetTriggerWordFromUserName(modelName);
        var trainModelDto = new TrainModelRequest(modelName, request.InputImageUrl, triggerWord);
        var trainModelResult = await _replicateService.TrainModelAsync(trainModelDto);
        ////test
        //var trainModelResponseDto = new TrainModelResponse("vytn2aq645rme0cq2q6az1erym", "starting", "https://api.replicate.com/v1/predictions/vytn2aq645rme0cq2q6az1erym/cancel");
        //var trainModelResult = BaseResponse<TrainModelResponse>.Success(trainModelResponseDto);
        ////end test code
        if (!trainModelResult.IsSuccess)
        {
            return BaseResponse<TrainModelResponse>.Fail(ErrorCode.TrainReplicateModelFail);
        }
        var modelTraining = new ModelTraining(
            Id: trainModelResult.Data.Id,
            ModelName: modelName,
            UserEmail: user.Email,
            UserName: user.Name,
            ImageUrl: request.InputImageUrl,
            TriggerWord: triggerWord,
            TrainingStatus: ModelTrainingStatus.InProgress,
            CreatedAt: DateTime.UtcNow,
            CanceledUrl: trainModelResult.Data.CancelUrl);
        await _modelTrainingRepository.CreateAsync(modelTraining);
        var result = new TrainModelResponse(
            trainModelResult.Data.Id,
            trainModelResult.Data.Status,
            trainModelResult.Data.CancelUrl);
        return BaseResponse<TrainModelResponse>.Success(result);
    }
}
