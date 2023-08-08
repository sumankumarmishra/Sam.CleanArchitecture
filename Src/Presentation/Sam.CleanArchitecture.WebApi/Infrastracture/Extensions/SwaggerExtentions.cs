﻿
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sam.CleanArchitecture.WebApi.Infrastracture.Extensions
{
    public static class SwaggerExtentions
    {
        private const string swaggerAuthorizationUrl = "/SwaggerAuthorization.js";
        public static IApplicationBuilder UseSwaggerWithVersioning(this IApplicationBuilder app)
        {
            IServiceProvider services = app.ApplicationServices;
            var provider = services.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    options.InjectJavascript(swaggerAuthorizationUrl);
                }
            });

            return app;
        }

        public static IServiceCollection AddSwaggerWithVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

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
                // setup.OperationFilter<AddRequiredHeaderParameter>();

            });
            services.ConfigureOptions<ConfigureSwaggerOptions>();

            return services;
        }
        public static void AddSwaggerAuthorization(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(swaggerAuthorizationUrl, async context =>
            {
                await context.Response.WriteAsync(temp());
                string temp()
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "Files", "SwaggerAuthorization.js");
                    return File.ReadAllText(path);
                }
            });
        }
        //public static void AddSwaggerAuthorization(this IEndpointRouteBuilder endpoints)
        //{
        //    endpoints.Map("/SwaggerAuthorization.js", () => temp());
        //    string temp()
        //    {
        //        var path = Path.Combine(Directory.GetCurrentDirectory(), "Files", "SwaggerAuthorization.js");
        //        return File.ReadAllText(path);
        //    }
        //}

    }
}
