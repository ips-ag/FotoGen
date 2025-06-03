using FotoGen.Domain.Entities.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FotoGen.Application.UseCases.UploadFile
{
    public class FileUploadCommand : IRequest<BaseResponse<string>>
    {
        public string UserId {  get; set; }
        public IFormFile File { get; set; }
    }
}
