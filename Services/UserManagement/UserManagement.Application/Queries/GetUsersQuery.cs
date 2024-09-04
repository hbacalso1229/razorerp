using AutoMapper;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Models;
using MediatR;

namespace UserManagement.Application.Queries
{
    public class GetUsersQuery : IRequest<UserResponse>
    {
        public GetUsersQuery(string role, Guid companyId)
        {
            Role = role;
            CompanyId = companyId;
        }

        public string Role { get; }

        public Guid CompanyId { get; }
    }

    public class GetUsereQueryHandler : IRequestHandler<GetUsersQuery, UserResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserQueryRepository _repository;

        public GetUsereQueryHandler(IUserQueryRepository repository, IConfigurationProvider configuration)
        {
            ArgumentNullException.ThrowIfNull(repository);
            ArgumentNullException.ThrowIfNull(configuration);

            _repository = repository;
            _mapper = configuration.CreateMapper();
        }

        public async Task<UserResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetUsersAsync(request.Role, request.CompanyId, cancellationToken);
        }
    }
}