using FotoGen.Domain.Entities.Models;

namespace FotoGen.Infrastructure.Replicate.GetTrainedModelStatus;

public class GetTrainedModelStatusMapper
{
    public static QueryModelTrainingStatus ToResponseDto(GetTrainedModelStatusResponse response)
    {
        return new QueryModelTrainingStatus(response.Id, response.Status, response.Model, response.Version);
    }
}
