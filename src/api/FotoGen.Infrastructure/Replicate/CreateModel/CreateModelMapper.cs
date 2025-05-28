using FotoGen.Domain.Entities.Models;
using FotoGen.Infrastructure.Settings;

namespace FotoGen.Infrastructure.Replicate.CreateModel
{
    public static class CreateModelMapper
    {
        public static CreateModelInput ToRequest(CreateModelRequest dto, ReplicateSetting settings)
        {
            return new CreateModelInput
            {
                Name = dto.Name,
                Description = dto.Description,
                Hardware = settings.Hardware,
                Owner = settings.Owner,
                Visibility = settings.Visibility
            };
        }
    }
}
