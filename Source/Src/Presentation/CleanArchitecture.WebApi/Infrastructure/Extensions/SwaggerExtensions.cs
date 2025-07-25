using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace CleanArchitecture.WebApi.Infrastructure.Extensions;

public static class SwaggerExtensions
{
    public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
    {

        app.UseSwagger();

        app.UseSwaggerUI();

        return app;
    }

    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(setup =>
        {
            setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
            });
            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    }, new List<string>()
                },
            });
        });

        return services;
    }
}