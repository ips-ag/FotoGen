using FotoGen.Domain.Entities.Models;

namespace FotoGen.Infrastructure.Replicate.GetTrainModelStatus;

public class GetTrainModelStatusMapper
{
    public static QueryModelTrainingStatus ToResponseDto(GetTrainModelStatusResponse response)
    {
        return new QueryModelTrainingStatus(response.Id, response.Status, response.Model, response.Version);
    }
}
