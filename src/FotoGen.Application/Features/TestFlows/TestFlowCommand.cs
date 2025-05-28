using FotoGen.Domain.Entities.Response;
using MediatR;

namespace FotoGen.Application.Features.TestFlows
{
    public class TestFlowCommand : IRequest<BaseResponse<bool>>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
