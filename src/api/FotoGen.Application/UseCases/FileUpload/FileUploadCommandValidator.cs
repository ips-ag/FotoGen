using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace FotoGen.Application.UseCases.FileUpload;

public class FileUploadCommandValidator : AbstractValidator<FileUploadCommand>
{
    public FileUploadCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required")
            .NotEmpty().WithMessage("File cannot be empty")
            .Must(BeAValidZipFile).WithMessage("Only ZIP files are allowed");
    }

    private bool BeAValidZipFile(IFormFile file)
    {
        var allowedExtensions = new[] { ".zip" };
        string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var allowedContentTypes = new[]
        {
            "application/zip", "application/x-zip-compressed", "multipart/x-zip", "application/octet-stream"
        };

        return allowedExtensions.Contains(fileExtension) &&
            allowedContentTypes.Contains(file.ContentType.ToLowerInvariant());
    }
}
