using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FotoGen.Externsions.OpenApi;

// https://raw.githubusercontent.com/microsoft/aspnet-api-versioning/master/samples/aspnetcore/SwaggerSample/ConfigureSwaggerOptions.cs
/// <summary>
///     Configures the Swagger generation options.
/// </summary>
/// <remarks>
///     This allows API versioning to define a Swagger document per API version after the
///     <see cref="IApiVersionDescriptionProvider" /> service has been resolved from the service container.
/// </remarks>
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConfigureSwaggerOptions" /> class.
    /// </summary>
    /// <param name="provider">
    ///     The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger
    ///     documents.
    /// </param>
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
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
                        AuthorizationUrl =
                            new Uri(
                                "https://login.microsoftonline.com/dcb58767-2d57-462f-82d5-552df1c47ccb/connect/authorize"),
                        Scopes = new Dictionary<string, string>
                        {
                            ["User.Read"] = "User.Read", ["FotoGen"] = "FotoGen"
                        }
                    }
                }
            });

        options.CustomSchemaIds(type => type.FullName);
        options.MapType<TimeSpan?>(() => new OpenApiSchema { Type = "string", Format = "string" });
        options.SupportNonNullableReferenceTypes();
        options.SchemaFilter<EnumSchemaFilter>();
        options.ParameterFilter<EnumParameterFilter>();
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
                //operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                //operation.Responses.Add("403", new Response { Description = "Forbidden" });
                var scopes = new[] { "User.Read FotoGen" };
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
