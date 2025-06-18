using FotoGen.Application.Configuration;
using FotoGen.Application.UseCases.CheckUserModelAvailable;
using FotoGen.Application.UseCases.GeneratePhoto;
using FotoGen.Application.UseCases.TrainModel;
using FotoGen.Extensions;
using FotoGen.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace FotoGen.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize("FotoGen")]
public class IntegrationController : ControllerBase
{
    private readonly IMediator _mediator;

    public IntegrationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [EnableRateLimiting(RateLimitPolicies.PhotoGeneration)]
    [HttpPost("generate-photo")]
    [ProducesResponseType<GeneratePhotoResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GeneratePhotoAsync(
        [FromBody] GeneratePhotoRequest request,
        CancellationToken cancel)
    {
        var command = new GeneratePhotoCommand
        {
            ModelName = request.ModelName, TriggerWord = request.TriggerWord, Prompt = request.Prompt
        };
        var result = await _mediator.Send(command, cancel);
        return result.ToActionResult();
    }

    [HttpGet("check-user-model-available")]
    [ProducesResponseType<bool>(StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckUserModelAvailableAsync(
        [FromQuery] string? modelName,
        CancellationToken cancel)
    {
        var query = new CheckUserModelAvailableQuery { ModelName = modelName };
        var result = await _mediator.Send(query, cancel);
        return result.ToActionResult();
    }
    [EnableRateLimiting(RateLimitPolicies.ModelTraining)]
    [HttpPost("train-model")]
    [ProducesResponseType<TrainModelResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> TrainModelAsync(TrainModelRequest request, CancellationToken cancel)
    {
        var command = new TrainModelCommand { InputImageUrl = request.ImageUrl };
        var result = await _mediator.Send(command, cancel);
        return result.ToActionResult();
    }
}
