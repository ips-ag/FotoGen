using System.Security.Claims;
using System.Threading.RateLimiting;
using FotoGen.Application.Configuration;
using FotoGen.Domain.Entities.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;

namespace FotoGen.Infrastructure.Settings
{
    public static class RateLimitConfiguration
    {
        private static RateLimitSettings _settings;
        public static void ConfigurePolicies(RateLimiterOptions options, IConfiguration configuration)
        {
            _settings = configuration.GetSection(RateLimitSettings.SectionName).Get<RateLimitSettings>() ?? throw new ArgumentNullException();

            options.AddPolicy(RateLimitPolicies.PhotoGeneration, context =>
                CreatePolicy(context, _settings.PhotoGeneration.PermitLimit, TimeSpan.FromDays(_settings.PhotoGeneration.WindowInDays)));

            options.AddPolicy(RateLimitPolicies.ModelTraining, context =>
                CreatePolicy(context, _settings.ModelTraining.PermitLimit, TimeSpan.FromDays(_settings.ModelTraining.WindowInDays)));

            options.OnRejected = (context, ct) => PolicyAwareRejectionHandler(context, ct);
        }
        private static RateLimitPartition<string> CreatePolicy(
            HttpContext context,
            int permitLimit,
            TimeSpan window)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new ArgumentNullException();
            return RateLimitPartition.GetSlidingWindowLimiter(
                partitionKey: userId,
                factory: _ => new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = permitLimit,
                    Window = window,
                    SegmentsPerWindow = 1
                });
        }
        private static async ValueTask PolicyAwareRejectionHandler(
        OnRejectedContext context,
        CancellationToken ct)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.HttpContext.Response.ContentType = "application/json";

            var policyName = context.HttpContext.GetEndpoint()?
                .Metadata.GetMetadata<EnableRateLimitingAttribute>()?
                .PolicyName;

            var response = policyName switch
            {
                RateLimitPolicies.PhotoGeneration => new
                {
                    isSuccess = false,
                    message = $"{_settings.PhotoGeneration.PermitLimit}",
                    errorCode = ErrorCode.ReachPhotoGenerationLimitation.ToString()
                },
                RateLimitPolicies.ModelTraining => new
                {
                    isSuccess = false,
                    message = $"{_settings.ModelTraining.PermitLimit}",
                    errorCode = ErrorCode.ReachTrainingLimitation.ToString()
                },
                _ => throw new ArgumentNullException()
            };

            await context.HttpContext.Response.WriteAsJsonAsync(response, ct);
        }
    }
}
