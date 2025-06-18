using System.Threading.RateLimiting;
using FotoGen.Application.Configuration;
using FotoGen.Domain.Entities.Response;
using FotoGen.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FotoGen.Infrastructure.Settings.RateLimit;

internal class ConfigureRateLimiterOptions : IConfigureOptions<RateLimiterOptions>
{
    public void Configure(RateLimiterOptions options)
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        options.AddPolicy(RateLimitPolicies.PhotoGeneration, CreatePhotoGenerationPolicy);
        options.AddPolicy(RateLimitPolicies.ModelTraining, CreateModelTrainingPolicy);
        options.OnRejected = OnRequestRejectedHandler;
    }

    private string CreatePartitionKey(HttpContext context)
    {
        var contextRepository = context.RequestServices.GetRequiredService<IRequestContextRepository>();
        var user = contextRepository.GetAsync().GetAwaiter().GetResult().User;
        return user.Id;
    }

    private RateLimitPartition<string> CreatePhotoGenerationPolicy(HttpContext context)
    {
        return CreatePolicy(
            context,
            _ =>
            {
                var settings = GetRateLimitSettings(context).PhotoGeneration;
                return new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = settings.PermitLimit, Window = settings.Window, SegmentsPerWindow = 1
                };
            });
    }

    private RateLimitPartition<string> CreateModelTrainingPolicy(HttpContext context)
    {
        return CreatePolicy(
            context,
            _ =>
            {
                var settings = GetRateLimitSettings(context).ModelTraining;
                return new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = settings.PermitLimit, Window = settings.Window, SegmentsPerWindow = 1
                };
            });
    }

    private RateLimitPartition<string> CreatePolicy(
        HttpContext context,
        Func<string, SlidingWindowRateLimiterOptions> optionsFactory)
    {
        return RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: CreatePartitionKey(context),
            factory: optionsFactory);
    }

    private async ValueTask OnRequestRejectedHandler(
        OnRejectedContext context,
        CancellationToken ct)
    {
        string? policyName = context.HttpContext.GetEndpoint()?
            .Metadata.GetMetadata<EnableRateLimitingAttribute>()?
            .PolicyName;
        var settings = GetRateLimitSettings(context.HttpContext);
        var problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
        var problem = problemDetailsFactory.CreateProblemDetails(
            httpContext: context.HttpContext,
            statusCode: StatusCodes.Status429TooManyRequests,
            title: "Rate limit exceeded",
            detail: $"You have exceeded the rate limit {policyName} for this resource. Please try again later.",
            instance: context.HttpContext.Request.Path,
            type: "https://httpstatuses.io/429");
        (string errorCode, string? message) = policyName switch
        {
            RateLimitPolicies.PhotoGeneration => (
                ErrorCode.ReachPhotoGenerationLimitation.ToString("G"),
                $"{settings.PhotoGeneration.PermitLimit}"
            ),
            RateLimitPolicies.ModelTraining => (
                ErrorCode.ReachTrainingLimitation.ToString("G"),
                $"{settings.ModelTraining.PermitLimit}"
            ),
            _ => (ErrorCode.Forbidden.ToString("G"), null)
        };
        problem.Extensions.TryAdd("errorCode", errorCode);
        problem.Extensions.TryAdd("message", message);
        await context.HttpContext.Response.WriteAsJsonAsync(problem, ct);
    }

    private static RateLimitSettings GetRateLimitSettings(HttpContext context)
    {
        return context.RequestServices.GetRequiredService<IOptions<RateLimitSettings>>().Value;
    }
}
