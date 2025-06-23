using FotoGen.Application.UseCases.FileUpload;
using FotoGen.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FotoGen.Controllers;

[Route("api/files")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("upload")]
    [RequestSizeLimit(200 * 1024 * 1024)]
    public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
    {
        var command = new FileUploadCommand { File = file };
        var result = await _mediator.Send(command, cancellationToken);
        return result.ToActionResult();
    }
}
