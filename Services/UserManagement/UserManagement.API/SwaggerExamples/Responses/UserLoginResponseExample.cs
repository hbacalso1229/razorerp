using UserManagement.Application.Common.Models;
using Swashbuckle.AspNetCore.Filters;

namespace UserManagement.API.SwaggerExamples.Responses
{
    public class UserLoginResponseExample : IExamplesProvider<UserLoginResponse>
    {
        public UserLoginResponse GetExamples()
        {
            return new UserLoginResponse
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFkbWluIiwicm9sZSI6IkFkbWluIiwiQ29tcGFueUlkIjoiZmEyOGQ5ODMtNjhkMy0xMWVmLTk1NGMtMDA1MDU2ODNjNzgzIiwibmJmIjoxNzI1MjUxOTEyLCJleHAiOjE3MjUyOTUxMDUsImlhdCI6MTcyNTI1MTkxMiwiaXNzIjoiQXV0aGVudGljYXRpb25Jc3N1ZXIiLCJhdWQiOiJDUk1BdWRpZW5jZSJ9.DQLh-CbAL8M8GY5ETE_x9MWEJ26ypm8CpIILN0QRvoc"
            };
        }
    }
}
