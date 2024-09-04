using UserManagement.API;
using UserManagement.API.Common.Filters;
using UserManagement.Application;
using UserManagement.Application.Common;
using UserManagement.Application.Common.Converters;
using UserManagement.Infrastructure;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    EnvironmentName = Environments.Development,
    WebRootPath = ""
});

#region Settings Configuration

string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Environment.EnvironmentName = env;

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.{env}.json", false, true)
    .AddJsonFile($"serilogsettings.{env}.json", false, true)
    .AddEnvironmentVariables();
#endregion

#region Serilog Configuration

builder.Host.UseSerilog();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
#endregion

#region Authentication Configuration

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddJWTAuthenticationServices(builder.Configuration);
#endregion

#region Swagger Configuration

builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
builder.Services.AddFluentValidationRulesToSwagger();

builder.Services.AddSwaggerGen(opt =>
{
    opt.EnableAnnotations();

    opt.UseInlineDefinitionsForEnums();
    opt.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    using (ServiceProvider serviceProvider = builder.Services.BuildServiceProvider())
    {
        IApiVersionDescriptionProvider provider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in provider.ApiVersionDescriptions)
        {
            opt.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = "User Management API",
                Version = description.ApiVersion.ToString(),
                Description = "User Management API",
            });
        }
    }

    opt.AddJWTOpenApiSecurityServices();

    //Api assembly xml documents
    List<string> apiXmlDocs = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"{Assembly.GetExecutingAssembly().GetName().Name.Split('.').FirstOrDefault()}*.xml").ToList();

    foreach (string filePath in apiXmlDocs)
    {
        opt.IncludeXmlComments(filePath);
    }

    opt.SchemaFilter<EnumerationToEnumTypeSchemaFilter>();
    opt.ExampleFilters();
});
#endregion

#region Controller Configuration

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers()
    .AddFluentValidation(config => config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()))
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new EnumerationTypeJsonConverter());
    });

builder.Services.AddEndpointsApiExplorer();
#endregion

#region Custom Configuration

builder.Services.AddApiVersioningServices();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
#endregion

#region Rate Limiting

builder.Services.AddRateLimiter(options => {

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    
    options.AddPolicy("FixedWindow", httpcontext => RateLimitPartition.GetFixedWindowLimiter(

        partitionKey: httpcontext.User.Identity?.Name?.ToString(),
        factory: _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(1)
        }));
});
#endregion

var app = builder.Build();

app.AddApplicationBuilderServices();

#region "Swagger Configuration"

app.UseSwagger();

IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwaggerUI(options =>
{
    foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"{description.GroupName}/swagger.json", $"User Management API {description.GroupName.ToLowerInvariant()}");
    }
});
#endregion

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();