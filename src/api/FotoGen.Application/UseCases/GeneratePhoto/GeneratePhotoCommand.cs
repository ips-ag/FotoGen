using FotoGen.Domain.Entities.Response;
using MediatR;

namespace FotoGen.Application.UseCases.GeneratePhoto;

public class GeneratePhotoCommand : IRequest<BaseResponse<GeneratePhotoResponse>>
{
    public string? ModelName { get; init; }
    public string? TriggerWord { get; init; }
    public required string Prompt { get; init; }
}
