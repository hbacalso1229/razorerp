using UserManagement.Application.Common;
using UserManagement.Application.Common.Exceptions;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Models;
using UserManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UserManagement.Application.Commands.AuthenticateUser
{
    public class AuthorizeUserCommand : LoginUserRequest, IRequest<string>
    {
    }

    public class AuthenticateUserCommandHandler : IRequestHandler<AuthorizeUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtOptions _jwtOptions;

        public AuthenticateUserCommandHandler(IUserRepository userRepository, 
            IOptions<JwtOptions> options)
        {
            ArgumentNullException.ThrowIfNull(userRepository);
            ArgumentNullException.ThrowIfNull(options);

            _userRepository = userRepository;
            _jwtOptions = options.Value;
        }

        public async Task<string> Handle(AuthorizeUserCommand request, CancellationToken cancellationToken)
        {
            User user = await _userRepository.FindAsync(x => x.Username.Equals(request.Username), cancellationToken: cancellationToken);
            if (user.Id == Guid.Empty) throw new UserNotFoundException(request.Username);

            var passwordVerificationResult  = new PasswordHasher<object?>().VerifyHashedPassword(null, user.Password, request.Password);
            if (passwordVerificationResult != PasswordVerificationResult.Success) throw new UnauthorizedAccessException();


            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_jwtOptions.SecretKey);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("CompanyId", user.CompanyId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(12),
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }       
    }
}
