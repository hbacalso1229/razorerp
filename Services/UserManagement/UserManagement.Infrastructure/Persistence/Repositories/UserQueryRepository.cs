using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Models;
using UserManagement.Domain.Enums;
using Dapper;

namespace UserManagement.Infrastructure.Persistence.Repositories
{
    public class UserQueryRepository : IUserQueryRepository
    {
        private readonly IDapperContext _context;

        public UserQueryRepository(IDapperContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            _context = context;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid userId, Guid companyId, CancellationToken cancellationToken = default)
        {
            //TODO: move to stored procedure
            string query = $@"SELECT Id AS UserId, Email, FirstName + ' ' + LastName AS DisplayName, CompanyId, Role
                                FROM [User]                                    
                                    WHERE Id = '{userId}' AND CompanyId = '{companyId}'  ;";

            UserDto user = await _context.QueryCustom(async dbConnection =>
            {
                return await dbConnection.QueryFirstOrDefaultAsync<UserDto>(query);
            });

            return user ?? new UserDto();
        }

        public async Task<UserResponse> GetUsersAsync(string role, Guid companyId, CancellationToken cancellationToken = default)
        {
            //TODO: move to stored procedure
            string query = $@"SELECT Id AS UserId, Email, FirstName + ' ' + LastName AS DisplayName, CompanyId, Role
                                FROM [User]                                    
                                    WHERE CompanyId = '{companyId}' {(!role.Equals(RoleType.Admin.Name) ? $" AND Role = '{role}' " : string.Empty)} ;";

            IEnumerable<UserDto> users =
            await _context.QueryCustom(async dbConnection =>
            {
                return await dbConnection.QueryAsync<UserDto>(query);
            });

            return new UserResponse() { Users = users.ToList() ?? new List<UserDto>() };
        }
    }
}
