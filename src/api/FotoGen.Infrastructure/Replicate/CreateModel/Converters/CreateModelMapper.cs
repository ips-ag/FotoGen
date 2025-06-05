using FotoGen.Domain.Entities.Models;
using FotoGen.Infrastructure.Replicate.Configuration;
using FotoGen.Infrastructure.Replicate.CreateModel.Models;

namespace FotoGen.Infrastructure.Replicate.CreateModel.Converters;

internal static class CreateModelMapper
{
    public static CreateModelInputModel ToRequest(CreateTrainedModelRequest dto, ReplicateSetting settings)
    {
        return new CreateModelInputModel
        {
            Name = dto.Name,
            Description = dto.Description,
            Hardware = settings.Training.Hardware,
            Owner = settings.Owner,
            Visibility = ToModel(settings.Training.Visibility)
        };
    }

    private static string ToModel(VisibilitySetting visibility)
    {
        return visibility.ToString("G").ToLowerInvariant();
    }
}
