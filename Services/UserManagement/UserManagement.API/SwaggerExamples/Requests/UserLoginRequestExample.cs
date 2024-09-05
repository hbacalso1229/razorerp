using Swashbuckle.AspNetCore.Filters;
using UserManagement.Application.Commands.AuthenticateUser;

namespace UserManagement.API.SwaggerExamples.Requests
{
    public class UserLoginRequestExample : IExamplesProvider<AuthorizeUserCommand>
    {
        public AuthorizeUserCommand GetExamples()
        {
            return new AuthorizeUserCommand
            {
                Username = "Admin",
                Password = "@dmin."
            };
        }
    }
}
