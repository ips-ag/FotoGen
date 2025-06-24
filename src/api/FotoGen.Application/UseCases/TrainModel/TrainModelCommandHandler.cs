using FluentValidation;
using FotoGen.Application.Interfaces;
using FotoGen.Domain.Entities.Models;
using FotoGen.Domain.Entities.Response;
using FotoGen.Domain.Repositories;
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
        string modelName = new ModelName(user);
        var trainedModel = await _replicateService.GetTrainedModelByNameAsync(modelName, cancellationToken);
        if (trainedModel is null)
        {
            var createReplicateModelRequestDto = new CreateTrainedModelRequest(modelName);
            var createModelResult = await _replicateService.CreateTrainedModelAsync(
                createReplicateModelRequestDto,
                cancellationToken);
            if (!createModelResult.IsSuccess)
            {
                return BaseResponse<TrainModelResponse>.Fail(ErrorCode.CreateReplicateModelFail);
            }
        }
        string triggerWord = new TriggerWord(user);
        var trainModelRequest = new TrainModelRequest(modelName, request.InputImageUrl, triggerWord);
        var trainModelResult = await _replicateService.CreateModelTrainingAsync(trainModelRequest, cancellationToken);
        if (!trainModelResult.IsSuccess)
        {
            return BaseResponse<TrainModelResponse>.Fail(ErrorCode.TrainReplicateModelFail);
        }
        var modelTraining = new ModelTraining(
            Id: trainModelResult.Data.Id,
            ModelName: modelName,
            UserEmail: user.Email,
            UserName: user.FullName,
            ImageUrl: request.InputImageUrl,
            TriggerWord: triggerWord,
            TrainingStatus: ModelTrainingStatus.InProgress,
            CreatedAt: DateTime.UtcNow,
            CanceledUrl: trainModelResult.Data.CancelUrl);
        await _modelTrainingRepository.CreateAsync(modelTraining, cancellationToken);
        var result = new TrainModelResponse(
            trainModelResult.Data.Id,
            trainModelResult.Data.Status,
            trainModelResult.Data.CancelUrl);
        return BaseResponse<TrainModelResponse>.Success(result);
    }
}
