using FotoGen;
using FotoGen.Application;
using FotoGen.Extensions.OpenApi;
using FotoGen.Extensions.OpenApi.Configuration;
using FotoGen.Extensions.Security;
using FotoGen.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddControllers();
builder.Services.AddApplicationDI();
builder.Services.AddInfrastructureDI(builder.Configuration);
builder.Services.AddCustomApiVersioning();
builder.Services.AddSwaggerGenRespectingCustomApiVersioning();
builder.Services.ConfigureAuthentication();
builder.Services.ConfigureAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .WithOrigins("http://localhost:8080") 
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
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
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseExceptionHandler("/api/errors");
app.Run();
