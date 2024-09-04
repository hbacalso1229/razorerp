using UserManagement.Application.Common.Exceptions.Base;

namespace UserManagement.Application.Common.Exceptions
{
    public class UserBadRequestException : BadRequestException
    {
        public UserBadRequestException(string message) : base(message)
        {
        }
    }
}
