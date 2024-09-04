using UserManagement.Application.Common.Exceptions.Base;

namespace UserManagement.Application.Common.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string user)
                    : base($"The user with identifier '{user}' was not found.")
        {
        }
    }
}
