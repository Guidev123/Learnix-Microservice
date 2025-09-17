using Learnix.Commons.WebApi.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Learnix.Commons.WebApi.Configurations
{
    public static class SwaggerConfiguration
    {
        public static WebApplicationBuilder AddSwaggerConfig(this WebApplicationBuilder builder)
        {
            var appSettingsSection = builder.Configuration.GetSection(nameof(KeycloakExtensions));
            var appSettings = appSettingsSection.Get<KeycloakExtensions>()
                ?? throw new InvalidOperationException("Keycloak settings not found.");

            builder.Services.Configure<KeycloakExtensions>(appSettingsSection);

            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));

                c.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(appSettings.AuthorizationUrl),
                            TokenUrl = new Uri(appSettings.TokenUrl),
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "OpenID Connect" },
                                { "profile", "Profile" }
                            }
                        }
                    }
                });

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Keycloak"
                            },
                            In = ParameterLocation.Header,
                            Name = "Bearer",
                            Scheme = "Bearer"
                        },
                        []
                    }
                };
                c.AddSecurityRequirement(securityRequirement);
            });

            return builder;
        }

        public static WebApplication UseSwaggerConfig(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.OAuthUsePkce();
                c.OAuthScopeSeparator(" ");
            });

            return app;
        }
    }
}