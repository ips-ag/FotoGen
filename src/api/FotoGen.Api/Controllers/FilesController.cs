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
        private readonly IUserContext _userContext;

        public FilesController(IMediator mediator, IUserContext userContext)
        {
            _mediator = mediator;
            _userContext = userContext;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
        {
            var userId = _userContext.UserId;
            var command = new FileUploadCommand { UserId = userId, File = file };
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }
    }
}
