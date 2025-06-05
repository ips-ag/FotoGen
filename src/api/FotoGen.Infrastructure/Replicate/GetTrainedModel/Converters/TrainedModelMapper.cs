using FotoGen.Domain.Entities.Models;
using FotoGen.Infrastructure.Replicate.GetTrainedModel.Models;

namespace FotoGen.Infrastructure.Replicate.GetTrainedModel.Converters;

internal static class TrainedModelMapper
{
    public static TrainedModel ToDomain(GetTrainedModelResponseModel model)
    {
        return new TrainedModel(model.Owner, model.Name, model.LatestVersion?.Id);
    }
}
