using AutoMapper;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Enums;

namespace UserManagement.Application.Common.Models
{
    public class CreateUserRequest : IMapFrom<User>
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Company Id
        /// </summary>
        public Guid CompanyId { get; set; }

        /// <summary>
        /// User role
        /// </summary>
        public RoleType Role { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateUserRequest, User>();               

            profile.CreateMap<string, string>().ConvertUsing(x => x ?? String.Empty);
        }
    }
}
