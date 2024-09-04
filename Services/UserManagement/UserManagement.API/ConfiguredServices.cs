using UserManagement.Application.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Dynamic;
using System.Net;
using System.Reflection;
using System.Text;

namespace UserManagement.API
{
    public static class ConfiguredServices
    {
        public static void AddApiVersioningServices(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.UseApiBehavior = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }

        public static void AddJWTAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            JwtOptions jwtOptions = configuration.GetSection("JwtSettings").Get<JwtOptions>();

            ArgumentNullException.ThrowIfNull(jwtOptions, nameof(jwtOptions));  

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Audience = jwtOptions.Audience;
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = delegate (JwtBearerChallengeContext context)
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        if (string.IsNullOrEmpty(context.Error))
                        {
                            context.Error = "Invalid Token";
                        }

                        if (string.IsNullOrEmpty(context.ErrorDescription))
                        {
                            context.ErrorDescription = "This request requires a valid JWT access token to be provided";
                        }

                        if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            SecurityTokenExpiredException ex = context.AuthenticateFailure as SecurityTokenExpiredException;
                            context.Response.Headers.Add("x-token-expired", ex.Expires.ToString("o"));
                            context.ErrorDescription = "The token expired on " + ex.Expires.ToString("o");
                        }

                        dynamic val = new ExpandoObject();
                        var anon = new
                        {
                            application = Assembly.GetExecutingAssembly().GetName().Name,
                            version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                            title = HttpStatusCode.Unauthorized.ToString(),
                            status = 401,
                            source = "JWT Aunthentication",
                            message = context.Error + ": " + context.ErrorDescription,
                            details = new { context.AuthenticateFailure }
                        };
                        val.data = new { };
                        val.success = false;
                        val.error = anon;
                        Log.Warning("Authentication response information {RequestMethod} {RequestPath} information", context.Request.Method, context.Request.Path);
                        Log.Warning("AuthenticateFailure", new
                        {
                            response = (object)val
                        });
                        Log.Debug("JWT Service Authentication Failure");
                        return context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            response = (object)val
                        }));
                    }
                };
            });
        }

        public static SwaggerGenOptions AddJWTOpenApiSecurityServices(this SwaggerGenOptions genOptions)
        {
            OpenApiSecurityScheme openApiSecurityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter your token in the text input below.\r\n\r\nExample: \"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            genOptions.AddSecurityDefinition(openApiSecurityScheme.Reference.Id, openApiSecurityScheme);
            genOptions.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                openApiSecurityScheme,
                new string[0]
            }});

            return genOptions;
        }
    }
}
