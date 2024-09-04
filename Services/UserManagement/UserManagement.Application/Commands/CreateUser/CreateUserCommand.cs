using AutoMapper;
using UserManagement.Application.Common.Exceptions;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Models;
using UserManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace UserManagement.Application.Commands.CreateUser
{
    public class CreateUserCommand : CreateUserRequest, IRequest<Guid>
    {
    }

    public class CreateTechnicianCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public CreateTechnicianCommandHandler(IUserRepository userRepository,
            IConfigurationProvider configuration)
        {
            ArgumentNullException.ThrowIfNull(userRepository);
            ArgumentNullException.ThrowIfNull(configuration);

            _userRepository = userRepository;
            _mapper = configuration.CreateMapper();
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            bool isExists = await _userRepository.ExistAysnc(u => u.CompanyId == request.CompanyId && u.Username.Equals(request.UserName) , cancellationToken);
            if (isExists) throw new UserBadRequestException($"The username with identifier '{request.UserName}' is already exists in the system. Please contact your system administrator for more information.");

            string hashPassword = new PasswordHasher<object?>().HashPassword(null, request.Password);
            request.Password = hashPassword;

            User user = _mapper.Map<User>(request);

            await _userRepository.AddAsync(user, cancellationToken);

            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return user.Id;
        }
    }
}
