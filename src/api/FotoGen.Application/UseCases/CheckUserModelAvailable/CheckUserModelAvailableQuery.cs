using FotoGen.Domain.Entities.Response;
using MediatR;

namespace FotoGen.Application.UseCases.CheckUserModelAvailable;

public class CheckUserModelAvailableQuery : IRequest<BaseResponse<bool>>
{
    public string? ModelName { get; init; }
}
