using UserManagement.Application.Common.Models;
using UserManagement.Domain.Enums;

namespace UserManagement.UnitTests.MockData
{
    public class UserData : IUserData<UserResponse>
    {
        public UserResponse GetData()
        {
            Guid userId = Guid.NewGuid();

            return new UserResponse
            {
                Users = new List<UserDto>
                {
                    new UserDto
                    {
                        UserId = Guid.NewGuid(),
                        Email = "helpdesk@gmail.com",
                        DisplayName = "Bill Gates",
                        CompanyId = Guid.NewGuid(),
                        Role = RoleType.Admin
                    },
                    new UserDto
                    {
                        UserId = Guid.NewGuid(),
                        DisplayName = "Steve Jobs",
                        CompanyId = Guid.NewGuid(),
                        Role = RoleType.User
                    }
                }
            };
        }
    }
}
