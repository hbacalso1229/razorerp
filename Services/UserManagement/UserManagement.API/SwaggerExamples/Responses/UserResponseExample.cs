using UserManagement.Application.Common.Models;
using UserManagement.Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace UserManagement.API.SwaggerExamples.Responses
{
    public class UserResponseExample : IExamplesProvider<UserResponse>
    {
        public UserResponse GetExamples()
        {
            return new UserResponse
            {
                Users = new List<UserDto>
                { 
                    new UserDto
                    {
                        UserId = Guid.NewGuid(),
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
