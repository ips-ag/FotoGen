using Asp.Versioning.ApiExplorer;
using FotoGen.Extensions.OpenApi.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FotoGen.Extensions.OpenApi;

// https://raw.githubusercontent.com/microsoft/aspnet-api-versioning/master/samples/aspnetcore/SwaggerSample/ConfigureSwaggerOptions.cs
/// <summary>
///     Configures the Swagger generation options.
/// </summary>
/// <remarks>
///     This allows API versioning to define a Swagger document per API version after the
///     <see cref="IApiVersionDescriptionProvider" /> service has been resolved from the service container.
/// </remarks>
internal class ConfigureGenSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private readonly IOptionsMonitor<SwaggerConfiguration> _options;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConfigureGenSwaggerOptions" /> class.
    /// </summary>
    /// <param name="provider">
    ///     The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger
    ///     documents.
    /// </param>
    /// <param name="options"></param>
    public ConfigureGenSwaggerOptions(
        IApiVersionDescriptionProvider provider,
        IOptionsMonitor<SwaggerConfiguration> options)
    {
        _provider = provider;
        _options = options;
    }

    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        // add a swagger document for each discovered API version
        // note: you might choose to skip or document deprecated API versions differently
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        options.CustomSchemaIds(type => type.FullName);
        options.MapType<TimeSpan?>(() => new OpenApiSchema { Type = "string", Format = "string" });
        options.SupportNonNullableReferenceTypes();
        options.SchemaFilter<EnumSchemaFilter>();
        options.ParameterFilter<EnumParameterFilter>();

        var settings = _options.CurrentValue.Authentication;
        if (settings?.AuthorizationUrl is null || settings.TokenUrl is null) return;
        options.OperationFilter<AuthorizeCheckOperationFilter>();
        options.AddSecurityDefinition(
            "oauth2",
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = settings.AuthorizationUrl,
                        TokenUrl = settings.TokenUrl,
                        Scopes = settings.Scopes?.ToDictionary(kvp => kvp.Name, kvp => kvp.Description)
                    }
                }
            });
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = $"FotoGen API {description.ApiVersion}",
            Version = description.ApiVersion.ToString(),
            Description = "FotoGen API"
        };
        if (description.IsDeprecated)
        {
            info.Description += " (This API version has been deprecated)";
        }
        return info;
    }

    private class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authorizeAttributes = new List<AuthorizeAttribute>();
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
            {
                authorizeAttributes.AddRange(
                    actionDescriptor.ControllerTypeInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>());
                authorizeAttributes.AddRange(
                    actionDescriptor.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>());
            }
            if (authorizeAttributes.Count > 0)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
                var scopes = new[] { "FotoGen" };
                var oAuthScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                };
                var requirement = new OpenApiSecurityRequirement { [oAuthScheme] = scopes };
                operation.Security.Add(requirement);
            }
        }
    }

    private class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;
            if (!type.IsEnum) return;
            schema.Extensions.Add(
                "x-ms-enum",
                new OpenApiObject
                {
                    ["name"] = new OpenApiString(type.Name), ["modelAsString"] = new OpenApiBoolean(false)
                });
        }
    }

    private class EnumParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            var type = context.ApiParameterDescription.Type;

            if (type.IsEnum)
            {
                parameter.Extensions.Add(
                    "x-ms-enum",
                    new OpenApiObject
                    {
                        ["name"] = new OpenApiString(type.Name), ["modelAsString"] = new OpenApiBoolean(false)
                    });
            }
        }
    }
}
