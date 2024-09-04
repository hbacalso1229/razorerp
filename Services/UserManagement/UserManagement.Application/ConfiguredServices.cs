using UserManagement.Application.Common;
using UserManagement.Application.Common.Behaviours;
using UserManagement.Application.Common.Middlewares;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace UserManagement.Application
{
    public static class ConfiguredServices
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<ApplicationLoggerMiddleware>();
            services.AddTransient<ExceptionHandlingMiddleware>();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(config => {
                config.AddProfile(new MappingProfile(Assembly.GetExecutingAssembly()));
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public static void AddApplicationBuilderServices(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ApplicationLoggerMiddleware>();

            builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
