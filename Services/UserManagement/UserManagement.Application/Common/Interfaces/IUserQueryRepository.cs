using UserManagement.Application.Common.Models;

namespace UserManagement.Application.Common.Interfaces
{
    public interface IUserQueryRepository
    {
        Task<UserResponse> GetUsersAsync(string role, Guid companyId, CancellationToken cancellationToken = default);

        Task<UserDto> GetUserByIdAsync(Guid userId, Guid companyId, CancellationToken cancellationToken = default);
    }
}
