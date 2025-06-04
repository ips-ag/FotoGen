using FluentValidation;
using FotoGen.Application.Helpers;
using FotoGen.Application.Interfaces;
using FotoGen.Domain.Entities.Response;
using FotoGen.Domain.Repositories;
using MediatR;

namespace FotoGen.Application.UseCases.GeneratePhoto;

public class GeneratePhotoCommandHandler : IRequestHandler<GeneratePhotoCommand, BaseResponse<GeneratePhotoResponse>>
{
    private readonly IReplicateService _replicateService;
    private readonly IDownloadClient _downloadClient;
    private readonly IRequestContextRepository _requestContextRepository;
    private readonly IValidator<GeneratePhotoCommand> _validator;

    public GeneratePhotoCommandHandler(
        IReplicateService replicateService,
        IDownloadClient downloadClient,
        IValidator<GeneratePhotoCommand> validator,
        IRequestContextRepository requestContextRepository)
    {
        _replicateService = replicateService;
        _downloadClient = downloadClient;
        _validator = validator;
        _requestContextRepository = requestContextRepository;
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
        var modelName = string.IsNullOrEmpty(request.ModelName)
            ? Helper.GetModelNameFromUserInfo(user)
            : request.ModelName.ToLower();
        var triggerWords = string.IsNullOrEmpty(modelName)
            ? user.GivenName
            : Helper.GetTriggerWordFromModelName(modelName);
        var promptWithTriggerWords = $"{triggerWords} {request.Prompt}";
        var replicateResponse = await _replicateService.GeneratePhotoAsync(promptWithTriggerWords, modelName);
        if (!replicateResponse.IsSuccess) return BaseResponse<GeneratePhotoResponse>.Fail(ErrorCode.GeneratePhotoFail);
        byte[] bytesImage = await _downloadClient.GetByteArrayAsync(replicateResponse.Data.StreamUrl);
        if (bytesImage.Length == 0)
        {
            return BaseResponse<GeneratePhotoResponse>.Fail(ErrorCode.ImageGenerationResponseEmpty);
        }
        var base64Image = Convert.ToBase64String(bytesImage);
        var result = new GeneratePhotoResponse
        {
            Base64Image = base64Image,
            OutputFormat = replicateResponse.Data.OutputFormat
        };
        return BaseResponse<GeneratePhotoResponse>.Success(result);
    }
}
