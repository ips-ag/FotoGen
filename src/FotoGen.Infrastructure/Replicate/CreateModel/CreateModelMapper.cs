using FotoGen.Common.Contracts.Replicated.CreateModel;
using FotoGen.Infrastructure.Settings;

namespace FotoGen.Infrastructure.Replicate.CreateModel
{
    public static class CreateModelMapper
    {
        public static CreateModelInput ToRequest(CreateReplicateModelRequestDto dto, ReplicateSetting settings)
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
