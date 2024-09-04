using UserManagement.Application.Common.Exceptions;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Domain.Entities;
using MediatR;

namespace UserManagement.Application.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            ArgumentNullException.ThrowIfNull(userRepository);

            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            User user = await _userRepository.FindAsync(n => n.Id == request.UserId, cancellationToken: cancellationToken);
            if (user.Id == Guid.Empty) throw new UserNotFoundException(request.UserId.ToString());

            await _userRepository.Remove(user);

            await _userRepository.UnitOfWork.SaveChangesAsync();

            return user.Id;
        }
    }
}
