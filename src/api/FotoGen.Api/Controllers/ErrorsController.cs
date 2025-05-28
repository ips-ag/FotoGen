using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FotoGen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        private readonly ILogger<ErrorsController> _logger;
        private readonly IWebHostEnvironment _env;
        public ErrorsController(ILogger<ErrorsController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }
        [Route("/errors")]
        [HttpPost]
        public IActionResult HandleError()
        {
            var exceptionHandler = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionHandler?.Error;
            _logger.LogError(exception?.Message, exception);
            var detail = _env.IsDevelopment() ? exception?.Message : "An unexpected error occurred. Please contact support.";
            return Problem(
                title: "An unexpected error occurred",
                detail: detail,
                statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }
}
