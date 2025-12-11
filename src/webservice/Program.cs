#region OpenHolidays API - Copyright (C) STÜBER SYSTEMS GmbH
/*    
 *    OpenHolidays API 
 *    
 *    Copyright (C) STÜBER SYSTEMS GmbH
 *
 *    This program is free software: you can redistribute it and/or modify
 *    it under the terms of the GNU Affero General Public License, version 3,
 *    as published by the Free Software Foundation.
 *
 *    This program is distributed in the hope that it will be useful,
 *    but WITHOUT ANY WARRANTY; without even the implied warranty of
 *    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *    GNU Affero General Public License for more details.
 *
 *    You should have received a copy of the GNU Affero General Public License
 *    along with this program. If not, see <http://www.gnu.org/licenses/>.
 *
 */
#endregion

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi;
using OpenHolidaysApi;
using OpenHolidaysApi.DataLayer;
using System.Collections;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Bind configuration
var appConfiguration = builder.Configuration.Get<AppConfiguration>();

// Enable cross-origin resource sharing 
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .WithMethods(WebRequestMethods.Http.Get)
              .WithHeaders(HeaderNames.Accept);
    });
});

// Add controller support
builder.Services
    .AddControllers(setup =>
    {
        setup.OutputFormatters.Add(new IcsOutputFormatter());
        setup.OutputFormatters.Add(new CsvOutputFormatter());
    })
    .AddJsonOptions(setup =>
    {
        setup.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        setup.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        setup.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers = { (JsonTypeInfo type_info) =>
                {
                    foreach (var property in type_info.Properties)
                    {
                        if (typeof(ICollection).IsAssignableFrom(property.PropertyType))
                        {
                            property.ShouldSerialize = (_, val) => val is ICollection collection && collection.Count > 0;
                        }
                    }
                }
            }
        };
    });

// Exception handling
builder.Services.AddProblemDetails();

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "OpenHolidays API v1",
            Version = "v1",
            Description = "Open Data API for public and school holidays",
            Contact = new OpenApiContact
            {
                Name = "The OpenHolidays API Project",
                Url = new Uri("https://www.openholidaysapi.org")
            },
            License = new OpenApiLicense
            {
                Name = "Open Database License",
                Url = new Uri("https://github.com/openpotato/openholidaysapi.data/blob/main/LICENSE")
            }
        });
    setup.EnableAnnotations();
    setup.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "OpenHolidaysApi.WebService.xml"));
    setup.OrderActionsBy((apiDesc) => apiDesc.RelativePath);
});

// Create a PostgreSQL data source 
await using var dataSource = DbDataSourceFactory.CreateDataSource(appConfiguration.Database);

// Add EF Core support
builder.Services.AddDbContext<AppDbContext>(options =>
{
  options.UseNpgsql(dataSource, providerOptions => providerOptions.EnableRetryOnFailure());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });
    app.UseStatusCodePages();
    app.UseExceptionHandler(new ExceptionHandlerOptions
    {
        ExceptionHandler = async (HttpContext context) =>
        {
            // Pass-through status codes from BadHttpRequestException. See: https://github.com/dotnet/aspnetcore/issues/43831
            var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
            var error = exceptionHandlerFeature?.Error;

            if (error is BadHttpRequestException badRequestEx)
            {
                context.Response.StatusCode = badRequestEx.StatusCode;
            }

            if (context.RequestServices.GetRequiredService<IProblemDetailsService>() is { } problemDetailsService)
            {
                await problemDetailsService.WriteAsync(new()
                {
                    HttpContext = context,
                    AdditionalMetadata = exceptionHandlerFeature?.Endpoint?.Metadata,
                    ProblemDetails = { Status = context.Response.StatusCode, Detail = error?.Message }
                });
            }
            else if (ReasonPhrases.GetReasonPhrase(context.Response.StatusCode) is { } reasonPhrase)
            {
                await context.Response.WriteAsync(reasonPhrase);
            }
        }
    });
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DocumentTitle = "OpenHolidays API";
    options.SwaggerEndpoint("v1/swagger.json", "OpenHolidays API v1");
});

app.UseCors();
app.MapControllers();
app.Run();  