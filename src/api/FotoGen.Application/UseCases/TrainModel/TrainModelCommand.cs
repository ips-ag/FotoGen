using FotoGen.Domain.Entities.Response;
using MediatR;

namespace FotoGen.Application.UseCases.TrainModel;

public class TrainModelCommand : IRequest<BaseResponse<TrainModelResponse>>
{
    public required string InputImageUrl { get; init; }
}
