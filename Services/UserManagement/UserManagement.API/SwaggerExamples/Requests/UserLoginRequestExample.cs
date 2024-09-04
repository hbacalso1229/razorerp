using UserManagement.Application.Common.Models;
using Swashbuckle.AspNetCore.Filters;

namespace UserManagement.API.SwaggerExamples.Requests
{
    public class UserLoginRequestExample : IExamplesProvider<LoginUserRequest>
    {
        public LoginUserRequest GetExamples()
        {
            return new LoginUserRequest
            {
                Username = "Admin",
                Password = "@dmin."
            };
        }
    }
}
