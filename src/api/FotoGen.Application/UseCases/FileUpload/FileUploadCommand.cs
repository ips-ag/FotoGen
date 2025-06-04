using FotoGen.Domain.Entities.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FotoGen.Application.UseCases.FileUpload;

public class FileUploadCommand : IRequest<BaseResponse<string>>
{
    public required IFormFile File { get; init; }
}
