using Microsoft.AspNetCore.Http;

namespace FotoGen.Application.Interfaces
{
    public interface IAzureStorageService
    {
        Task<string> UploadFileAsync(string userId, IFormFile file);
    }
}
