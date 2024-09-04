using Swashbuckle.AspNetCore.Filters;

namespace UserManagement.API.SwaggerExamples.Responses
{
    public class CreateUserResponseExample : IExamplesProvider<Guid>
    {
        public Guid GetExamples()
        {
            return Guid.NewGuid();
        }
    }
}
