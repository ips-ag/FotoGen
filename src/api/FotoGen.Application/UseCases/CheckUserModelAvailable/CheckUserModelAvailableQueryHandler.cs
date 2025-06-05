using FluentValidation;
using FotoGen.Application.Interfaces;
using FotoGen.Domain.Entities.Models;
using FotoGen.Domain.Entities.Response;
using FotoGen.Domain.Repositories;
using MediatR;

namespace FotoGen.Application.UseCases.CheckUserModelAvailable;

public class CheckUserModelAvailableQueryHandler : IRequestHandler<CheckUserModelAvailableQuery, BaseResponse<bool>>
{
    private readonly IReplicateService _replicateService;
    private readonly IValidator<CheckUserModelAvailableQuery> _validator;
    private readonly IRequestContextRepository _requestContextRepository;

    public CheckUserModelAvailableQueryHandler(
        IReplicateService replicateService,
        IValidator<CheckUserModelAvailableQuery> validator,
        IRequestContextRepository requestContextRepository)
    {
        _replicateService = replicateService;
        _validator = validator;
        _requestContextRepository = requestContextRepository;
    }

    public async Task<BaseResponse<bool>> Handle(
        CheckUserModelAvailableQuery request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BaseResponse<bool>.Fail(validationResult.ToDictionary());
        }
        var user = (await _requestContextRepository.GetAsync()).User;
        var ownerModelName = new ModelName(user);
        string modelName = request.ModelName?.ToLower() ?? ownerModelName;
        var trainedModel = await _replicateService.GetTrainedModelByNameAsync(modelName, cancellationToken);
        return trainedModel?.CanTrain == true
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Fail(ErrorCode.ReplicateModelNotFound);
    }
}
