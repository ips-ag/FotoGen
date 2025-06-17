using FluentValidation;
using FotoGen.Application.Interfaces;
using FotoGen.Domain.Consts;
using FotoGen.Domain.Entities.Models;
using FotoGen.Domain.Entities.Response;
using FotoGen.Domain.Repositories;
using MediatR;

namespace FotoGen.Application.UseCases.GeneratePhoto;

public class GeneratePhotoCommandHandler : IRequestHandler<GeneratePhotoCommand, BaseResponse<GeneratePhotoResponse>>
{
    private readonly IReplicateService _replicateService;
    private readonly IDownloadClient _downloadClient;
    private readonly IRequestContextRepository _requestContextRepository;
    private readonly IUsageLimitationRepository _usageLimitationRepository;
    private readonly IValidator<GeneratePhotoCommand> _validator;

    public GeneratePhotoCommandHandler(
        
        IReplicateService replicateService,
        IDownloadClient downloadClient,
        IValidator<GeneratePhotoCommand> validator,
        IRequestContextRepository requestContextRepository,
        IUsageLimitationRepository usageLimitationRepository)
    {
        _replicateService = replicateService;
        _downloadClient = downloadClient;
        _validator = validator;
        _requestContextRepository = requestContextRepository;
        _usageLimitationRepository = usageLimitationRepository;
    }

    public async Task<BaseResponse<GeneratePhotoResponse>> Handle(
        GeneratePhotoCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BaseResponse<GeneratePhotoResponse>.Fail(validationResult.ToDictionary());
        }
        var user = (await _requestContextRepository.GetAsync()).User;
        var userUsage = await _usageLimitationRepository.GetUserUsageAsync(user.Id, DateOnly.FromDateTime(DateTime.UtcNow));
        if (userUsage?.PhotoGenerationCount > ConstValue.LimitPhotoGenerationUsageInDay) return BaseResponse<GeneratePhotoResponse>.Fail(ErrorCode.ReachPhotoGenerationLimitation);
        string modelName = string.IsNullOrEmpty(request.ModelName) ? new ModelName(user) : request.ModelName.ToLower();
        var trainedModel = await _replicateService.GetTrainedModelByNameAsync(modelName, cancellationToken);
        if (trainedModel?.CanTrain != true) return BaseResponse<GeneratePhotoResponse>.Fail(ErrorCode.ReplicateModelNotFound);
        string triggerWord = string.IsNullOrEmpty(request.TriggerWord) ? new TriggerWord(user) : request.TriggerWord;
        string prompt = request.Prompt.Contains(triggerWord, StringComparison.OrdinalIgnoreCase)
            ? request.Prompt
            : $"{triggerWord} {request.Prompt}";
        var replicateResponse = await _replicateService.GeneratePhotoAsync(prompt, trainedModel, cancellationToken);
        if (!replicateResponse.IsSuccess) return BaseResponse<GeneratePhotoResponse>.Fail(ErrorCode.GeneratePhotoFail);
        byte[] bytesImage = await _downloadClient.GetByteArrayAsync(replicateResponse.Data.StreamUrl);
        if (bytesImage.Length == 0) return BaseResponse<GeneratePhotoResponse>.Fail(ErrorCode.ImageGenerationResponseEmpty);
        string base64Image = Convert.ToBase64String(bytesImage);
        var result = new GeneratePhotoResponse
        {
            Base64Image = base64Image, OutputFormat = replicateResponse.Data.OutputFormat
        };
        userUsage ??= new UserUsage(user);
        userUsage.PhotoGenerationCount++;
        await _usageLimitationRepository.UpSetAsync(userUsage);
        return BaseResponse<GeneratePhotoResponse>.Success(result);
    }
}
