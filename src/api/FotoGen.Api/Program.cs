using System.Reflection;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using FotoGen.Application;
using FotoGen.Extensions.Cors;
using FotoGen.Extensions.OpenApi;
using FotoGen.Extensions.OpenApi.Configuration;
using FotoGen.Extensions.OpenTelemetry;
using FotoGen.Extensions.Security;
using FotoGen.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCustomApiVersioning();
builder.Services.AddSwaggerGenRespectingCustomApiVersioning();
builder.Services.ConfigureOpenTelemetry(builder.Environment);
builder.Services.ConfigureAuthentication();
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureCors();
builder.Services.AddApplication().AddInfrastructure();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(setup =>
    {
        setup.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
        setup.RouteTemplate = "swagger/{documentName}/swagger.json";
        setup.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
        {
            swaggerDoc.Servers =
            [
                new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}", Description = "Default" }
            ];
        });
    });
    app.UseSwaggerUI(options =>
    {
        var settings = app.Services.GetRequiredService<IOptions<SwaggerConfiguration>>().Value;
        options.OAuthClientId(settings.Authentication?.ClientId);
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions.OrderByDescending(x => x.ApiVersion))
        {
            var endpointUrl = $"{description.GroupName}/swagger.json";
            options.SwaggerEndpoint(endpointUrl, description.GroupName.ToUpperInvariant());
        }
    });
}
app.UseCorsMiddleware();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseExceptionHandler("/api/errors");
app.Run();
