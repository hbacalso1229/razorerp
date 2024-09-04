using UserManagement.Application.Common.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Persistence.Base;
using UserManagement.Infrastructure.Persistence.DbContexts;
using Microsoft.Extensions.Logging;

namespace UserManagement.Infrastructure.Persistence.Repositories
{
    public class UserRepository : RepositoryBase<UserManagementDbContext, User>, IUserRepository
    {
        public UserRepository(UserManagementDbContext dbContext, ILogger<UserRepository> logger) : base(dbContext, logger)
        {
        }
    }
}
