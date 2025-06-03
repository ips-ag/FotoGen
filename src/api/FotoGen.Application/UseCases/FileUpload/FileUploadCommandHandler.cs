using FluentValidation;
using FotoGen.Application.Interfaces;
using FotoGen.Application.UseCases.UploadFile;
using FotoGen.Domain.Entities.Response;
using MediatR;

namespace FotoGen.Application.UseCases.FileUpload
{
    public class FileUploadCommandHandler : IRequestHandler<FileUploadCommand, BaseResponse<string>>
    {
        private readonly IAzureStorageService _azureStorageService;
        private readonly IValidator<FileUploadCommand> _validator;
        public FileUploadCommandHandler(
            IAzureStorageService azureStorageService,
            IValidator<FileUploadCommand> validator)
        {
            _azureStorageService = azureStorageService;
            _validator = validator;
        }
        public async Task<BaseResponse<string>> Handle(FileUploadCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BaseResponse<string>.Fail(validationResult.ToDictionary());
            }
            var result = await _azureStorageService.UploadFileAsync(request.UserId, request.File);
            return BaseResponse<string>.Success(result);
        }
    }
}
