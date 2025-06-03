using System.Security.Claims;
using FotoGen.Application.UseCases.UploadFile;
using FotoGen.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FotoGen.Controllers
{
    [Route("api/files")]
    [ApiController]
    [RequestSizeLimit(100 * 1024 * 1024)]
    public class FilesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FilesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var command = new FileUploadCommand { UserId = userId, File = file };
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }
    }
}
