using FotoGen.Application.UseCases.GeneratePhoto;
using FotoGen.Application.UseCases.GetUserAvailableModel;
using FotoGen.Application.UseCases.TrainModel;
using FotoGen.Externsions;
using FotoGen.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FotoGen.Controllers;

[Route("api/integration")]
[ApiController]
public class IntegrationController : ControllerBase
{
    private readonly IMediator _mediator;

    public IntegrationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("generate-photo")]
    public async Task<IActionResult> GeneratePhotoAsync(GeneratePhotoRequest request)
    {
        //TODO: Get from jwt token
        if (string.IsNullOrEmpty(request.ModelName))
        {
            //Get from userId
            request.ModelName = "e8f678ce-1087-4411-a887-6fa6622e1a42";
        }
        //TODO: Request validation
        var command = new GeneratePhotoCommand { ModelName = request.ModelName, Prompt = request.Prompt };
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }

    [HttpGet("check-user-model-available")]
    public async Task<IActionResult> CheckUserModelAvailableAsync()
    {
        //TODO: Get from jwt
        var userId = "e8f678ce-1087-4411-a887-6fa6622e1a42";
        var query = new CheckUserModelAvailableQuery { ModelName = userId };
        var result = await _mediator.Send(query);
        return result.ToActionResult();
    }

    [HttpPost("train-model")]
    public async Task<IActionResult> TrainModelAsync(TrainModelRequest request)
    {
        //TODO: Get from jwt token
        var userName = "Cuong";
        var userId = "e8f678ce-1087-4411-a887-6fa6622e1a42";
        var email = "abc@gmail.com";
        //TODO: Request validation
        var command = new TrainModelCommand
        {
            ModelName = userId, UserName = userName, UserEmail = email, InputImageUrl = request.ImageUrl
        };
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
}
