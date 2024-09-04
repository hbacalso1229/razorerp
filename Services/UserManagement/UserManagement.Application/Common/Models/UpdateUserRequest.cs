using AutoMapper;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Common.Models
{
    public class UpdateUserRequest : CreateUserRequest, IMapFrom<User>
    {
        /// <summary>
        /// Company Id
        /// </summary>
        public Guid UserId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateUserRequest, User>();

            profile.CreateMap<string, string>().ConvertUsing(x => x ?? String.Empty);
        }
    }
}
