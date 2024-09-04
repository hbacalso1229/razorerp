using UserManagement.Application.Commands.CreateUser;
using UserManagement.Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace UserManagement.API.SwaggerExamples.Requests
{
    public class CreateUserRequestExample : IExamplesProvider<CreateUserCommand>
    {
        public CreateUserCommand GetExamples()
        {
            return new CreateUserCommand
            {   
                Email = "helpdesk@gmail.com",
                FirstName = "Bill",
                LastName = "Gates",
                UserName = "superuser",
                CompanyId = Guid.NewGuid(),
                Password = "@!#superuser123.",
                Role = RoleType.Admin
            };
        }
    }
}
