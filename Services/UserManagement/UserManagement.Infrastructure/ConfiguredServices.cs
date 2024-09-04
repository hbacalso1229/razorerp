using UserManagement.Application.Common.Interfaces;
using UserManagement.Infrastructure.Common.Handlers;
using UserManagement.Infrastructure.Persistence;
using UserManagement.Infrastructure.Persistence.DbContexts;
using UserManagement.Infrastructure.Persistence.Repositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserManagement.Infrastructure
{
    public static class ConfiguredServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetSection("DatabaseSettings:ConnectionString")?.Value;

            //Reads
            services.AddScoped<IDapperContext>(provider => new DapperContext(connectionString));

            //Writes
            services.AddDbContext<UserManagementDbContext>(options =>
            {
                options.UseSqlServer(connectionString, opt =>
                {
                    opt.MigrationsAssembly(typeof(UserManagementDbContext).Assembly.FullName);
                    opt.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });                
            },
                ServiceLifetime.Scoped
            );
            services.AddScoped<ICRMDbContext>(provider => provider.GetRequiredService<UserManagementDbContext>());
            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<UserManagementDbContext>());

            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserQueryRepository, UserQueryRepository>();

            SqlMapper.AddTypeHandler(new RoleTypeHandler());
            SqlMapper.AddTypeHandler(new GuidTypeHandler());

            return services;
        }
    }
}
