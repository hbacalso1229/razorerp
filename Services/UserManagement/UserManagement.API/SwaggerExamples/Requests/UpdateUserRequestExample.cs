using UserManagement.Application.Commands.UpdateUser;
using UserManagement.Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace UserManagement.API.SwaggerExamples.Requests
{
    public class UpdateUserRequestExample : IExamplesProvider<UpdateUserCommand>
    {
        public UpdateUserCommand GetExamples()
        {
            return new UpdateUserCommand
            {
                UserId = new Guid("e818dc7b-68f6-11ef-954c-00505683c783"),
                Email = "helpdesk@gmail.com",
                FirstName = "Bill",
                LastName = "Gates",
                UserName = $"Admin",
                CompanyId = new Guid("fa28d983-68d3-11ef-954c-00505683c783"),
                Password = "@dmin.",
                Role = RoleType.Admin
            };
        }
    }
}
