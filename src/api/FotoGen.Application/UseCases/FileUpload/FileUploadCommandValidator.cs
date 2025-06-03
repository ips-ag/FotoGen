using FluentValidation;
using FotoGen.Application.UseCases.UploadFile;
using Microsoft.AspNetCore.Http;

namespace FotoGen.Application.UseCases.FileUpload
{
    public class FileUploadCommandValidator : AbstractValidator<FileUploadCommand>
    {
        public FileUploadCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");
            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required")
                .NotEmpty().WithMessage("File cannot be empty")
                .Must(BeAValidZipFile).WithMessage("Only ZIP files are allowed");
        }

        private bool BeAValidZipFile(IFormFile file)
        {
            if (file == null)
                return false;

            var allowedExtensions = new[] { ".zip" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowedContentTypes = new[]
            {
                "application/zip",
                "application/x-zip-compressed",
                "multipart/x-zip",
                "application/octet-stream"
            };

            return allowedExtensions.Contains(fileExtension) &&
                   allowedContentTypes.Contains(file.ContentType.ToLowerInvariant());
        }
    }
}
