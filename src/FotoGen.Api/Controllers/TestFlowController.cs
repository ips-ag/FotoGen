using FotoGen.Application.Features.TestFlows;
using FotoGen.Externsions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FotoGen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestFlowController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TestFlowController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("TestFlow")]
        public async Task<IActionResult> TestFlow()
        {
            var command = new TestFlowCommand { Name = "", Description = "" };
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
