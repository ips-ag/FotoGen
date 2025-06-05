using FotoGen.Domain.Entities.Images;
using FotoGen.Infrastructure.Replicate.UseModel.Models;

namespace FotoGen.Infrastructure.Replicate.UseModel.Converters;

internal static class UseModelMapper
{
    public static UseModelRequestModel ToModel(string prompt, string version)
    {
        return new UseModelRequestModel { Version = version, Input = new InputModel { Prompt = prompt } };
    }

    public static GenerateImageResponse ToDomain(UseModelResponseModel responseModel)
    {
        return new GenerateImageResponse(responseModel.Urls.Stream, responseModel.Input.OutputFormat);
    }
}
