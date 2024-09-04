using AutoMapper;
using UserManagement.Application.Common.Exceptions;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Models;
using UserManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace UserManagement.Application.Commands.UpdateUser
{
    public class UpdateUserCommand : UpdateUserRequest, IRequest<Guid>
    {
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository,
            IConfigurationProvider configuration)
        {
            ArgumentNullException.ThrowIfNull(userRepository);
            ArgumentNullException.ThrowIfNull(configuration);

            _userRepository = userRepository;
            _mapper = configuration.CreateMapper();
        }

        public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            User user = await _userRepository.FindAsync(n => n.Id == request.UserId, cancellationToken: cancellationToken);
            if (user.Id == Guid.Empty) throw new UserNotFoundException(request.UserId.ToString());

            string hashPassword = new PasswordHasher<object?>().HashPassword(null, request.Password);

            _mapper.Map(request, user);
            user.SetPassword(hashPassword);            

            await _userRepository.UpdateAsync(user, cancellationToken);

            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return user.Id;
        }
    }
}
