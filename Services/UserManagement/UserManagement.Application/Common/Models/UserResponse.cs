namespace UserManagement.Application.Common.Models
{
    public class UserResponse
    {
        public UserResponse()
        {
            Users = new List<UserDto>();
        }
        public IList<UserDto> Users { get; set; }
    }
}
