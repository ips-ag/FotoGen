using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FotoGen.Controllers;

[Route("api/[controller]")]
[Authorize("FotoGen")]
[ApiController]
public class ErrorsController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public ErrorsController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [Route("")]
    [HttpPost]
    [ProducesResponseType<ProblemDetails>((int)HttpStatusCode.InternalServerError)]
    public IActionResult HandleError()
    {
        var exceptionHandler = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionHandler?.Error;
        string? detail = _env.IsDevelopment()
            ? exception?.Message
            : "An unexpected error occurred. Please contact support.";
        return Problem(
            title: "An unexpected error occurred",
            detail: detail,
            statusCode: (int)HttpStatusCode.InternalServerError);
    }
}
