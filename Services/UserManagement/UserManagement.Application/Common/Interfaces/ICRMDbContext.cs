using UserManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Application.Common.Interfaces
{
    public interface ICRMDbContext
    {
        public DbSet<User> Users { get; set; }
    }
}
