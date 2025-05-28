using FotoGen.Common.Contracts.Replicated.UseModel;
using FotoGen.Infrastructure.Settings;

namespace FotoGen.Infrastructure.Replicate.UseModel
{
    public static class UseModelMapper
    {
        public static UseModelInput ToInput(string prompt, string modelName, ReplicateSetting settings)
        {
            return new UseModelInput
            {
                Version = $"{settings.Owner}/{modelName}:{settings.Version}",
                Input = new Input
                {
                    Model = settings.Mode,
                    Prompt = prompt,
                    OutputFormat = settings.OutputFormat
                }
            };
        }
        public static UseModelResponseDto ToResponseDto(UseModelResponse userModelResponse)
        {
            return new UseModelResponseDto
            {
                StreamUrl = userModelResponse.Urls.Stream,
                OutputFormat = userModelResponse.Input.OutputFormat
            };
        }

    }
}
