using FotoGen.Domain.Entities.Images;
using FotoGen.Infrastructure.Settings;

namespace FotoGen.Infrastructure.Replicate.UseModel;

public static class UseModelMapper
{
    public static UseModelRequestModel ToModel(string prompt, string modelName, ReplicateSetting settings)
    {
        return new UseModelRequestModel
        {
            Version = $"{settings.Owner}/{modelName}:{settings.Version}",
            Input = new Input { Model = settings.Mode, Prompt = prompt, OutputFormat = settings.OutputFormat }
        };
    }

    public static GenerateImageResponse ToDomain(UseModelResponseModel responseModel)
    {
        return new GenerateImageResponse(responseModel.Urls.Stream, responseModel.Input.OutputFormat);
    }
}
