using FotoGen.Common.Contracts.Replicated.GetTrainedModelStatus;

namespace FotoGen.Infrastructure.Replicate.GetTrainModelStatus
{
    public class GetTrainModelStatusMapper
    {
        public static GetTrainedModelStatusResponseDto ToResponseDto(GetTrainModelStatusResponse response)
        {
            return new GetTrainedModelStatusResponseDto
            {
                Id = response.Id,
                Status = response.Status,
                Model = response.Model,
                Version = response.Version,
            };
        }
    }
}
