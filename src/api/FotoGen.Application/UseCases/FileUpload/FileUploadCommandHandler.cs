using FluentValidation;
using FotoGen.Application.Interfaces;
using FotoGen.Domain.Entities.Response;
using FotoGen.Domain.Repositories;
using MediatR;

namespace FotoGen.Application.UseCases.FileUpload;

public class FileUploadCommandHandler : IRequestHandler<FileUploadCommand, BaseResponse<string>>
{
    private readonly IAzureStorageService _azureStorageService;
    private readonly IRequestContextRepository _requestContextRepository;
    private readonly IValidator<FileUploadCommand> _validator;

    public FileUploadCommandHandler(
        IAzureStorageService azureStorageService,
        IValidator<FileUploadCommand> validator,
        IRequestContextRepository requestContextRepository)
    {
        _azureStorageService = azureStorageService;
        _validator = validator;
        _requestContextRepository = requestContextRepository;
    }

    public async Task<BaseResponse<string>> Handle(FileUploadCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BaseResponse<string>.Fail(validationResult.ToDictionary());
        }
        var context = await _requestContextRepository.GetAsync();
        var user = context.User;
        string result = await _azureStorageService.UploadFileAsync(user.Id, request.File);
        return BaseResponse<string>.Success(result);
    }
}
