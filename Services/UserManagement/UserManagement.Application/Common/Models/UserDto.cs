using AutoMapper;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Enums;

namespace UserManagement.Application.Common.Models
{
    public class UserDto : IMapFrom<User>
    {
        /// <summary>
        /// User name
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Display name
        /// </summary>
        public string DisplayName { get; set; }

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
            profile.CreateMap<User, UserDto>();

            profile.CreateMap<string, string>().ConvertUsing(x => x ?? String.Empty);
        }
    }
}
