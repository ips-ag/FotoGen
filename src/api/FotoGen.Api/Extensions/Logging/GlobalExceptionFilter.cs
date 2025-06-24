using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace FotoGen.Extensions.Logging;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _env;
    private readonly ProblemDetailsFactory _problemFactory;
    private readonly ExceptionStatusCodeMapper _mapper;

    private bool IncludeDetails
    {
        get => _env.IsDevelopment();
    }

    public GlobalExceptionFilter(
        IHostEnvironment env,
        ExceptionStatusCodeMapper mapper,
        ProblemDetailsFactory problemFactory)
    {
        _env = env;
        _mapper = mapper;
        _problemFactory = problemFactory;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.HttpContext.Response.HasStarted) return;
        var e = context.Exception;
        int statusCode = _mapper.GetStatusCode(e);
        string title = e.Message;
        string? detail = IncludeDetails ? e.ToString() : null;
        var problem = _problemFactory.CreateProblemDetails(
            httpContext: context.HttpContext,
            statusCode: statusCode,
            title: title,
            type: $"https://httpstatuses.io/{statusCode}",
            detail: detail);
        var result = new ObjectResult(problem);
        result.ContentTypes.Add("application/problem+json");
        result.StatusCode = statusCode;
        context.Result = result;
        context.ExceptionHandled = true;
    }
}
