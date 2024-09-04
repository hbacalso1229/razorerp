using AutoMapper;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Models;
using MediatR;

namespace UserManagement.Application.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public GetUserByIdQuery(Guid userId, Guid companyId)
        {
            UserId = userId;
            CompanyId = companyId;
        }

        public Guid UserId { get; }

        public Guid CompanyId { get; }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IMapper _mapper;
        private readonly IUserQueryRepository _repository;

        public GetUserByIdQueryHandler(IUserQueryRepository repository, IConfigurationProvider configuration)
        {
            ArgumentNullException.ThrowIfNull(repository);
            ArgumentNullException.ThrowIfNull(configuration);

            _repository = repository;
            _mapper = configuration.CreateMapper();
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetUserByIdAsync(request.UserId, request.CompanyId, cancellationToken);
        }
    }
}
