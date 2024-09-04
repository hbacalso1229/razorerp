using UserManagement.Application.Common.Interfaces;
using UserManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace UserManagement.Infrastructure.Persistence.DbContexts
{
    public class UserManagementDbContext : DbContext, ICRMDbContext, IUnitOfWork
    {
        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            int result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

