#region OpenHolidays API - Copyright (C) 2023 STÜBER SYSTEMS GmbH
/*    
 *    OpenHolidays API 
 *    
 *    Copyright (C) 2023 STÜBER SYSTEMS GmbH
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

using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using OpenHolidaysApi;
using OpenHolidaysApi.DataLayer;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Bind configuration
var appConfiguration = builder.Configuration.Get<AppConfiguration>();

// Enable cross-origin resource sharing 
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .WithMethods("GET")
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
    });

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
    setup.UseDateOnlyTimeOnlyStringConverters();
});

// Add EF Core support
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.BuildDbContextOptions(appConfiguration.Database);
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
    app.UseExceptionHandler("/error");
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