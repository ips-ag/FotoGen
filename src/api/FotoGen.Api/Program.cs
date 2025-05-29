using Asp.Versioning.ApiExplorer;
using FotoGen.Application;
using FotoGen.Externsions.OpenApi;
using FotoGen.Infrastructure;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);
builder.Services.AddControllers();
builder.Services.AddApplicationDI();
builder.Services.AddInfrastructureDI(builder.Configuration);
builder.Services.AddOpenApi();
builder.Services.AddCustomApiVersioning();
builder.Services.AddSwaggerGenRespectingCustomApiVersioning();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger(setup =>
    {
        setup.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
        setup.RouteTemplate = "swagger/{documentName}/swagger.json";
        setup.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
        {
            swaggerDoc.Servers =
            [
                new OpenApiServer {
                    Url = $"{httpReq.Scheme}://{httpReq.Host.Value}",
                    Description = "Default"
                }
            ];
        });
    });
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId("76f75887-be52-49c6-882d-200da320be23");
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions.OrderByDescending(x => x.ApiVersion))
        {
            var endpointUrl = $"{description.GroupName}/swagger.json";
            options.SwaggerEndpoint(endpointUrl, description.GroupName.ToUpperInvariant());
        }
    });
}
app.MapControllers();
app.UseExceptionHandler("/errors");
app.Run();
