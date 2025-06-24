using Microsoft.AspNetCore.Mvc;

namespace FotoGen.Controllers;

[Route("")]
[ApiController]
public class KeepAliveController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IResult Get()
    {
        return Results.Ok();
    }
}
