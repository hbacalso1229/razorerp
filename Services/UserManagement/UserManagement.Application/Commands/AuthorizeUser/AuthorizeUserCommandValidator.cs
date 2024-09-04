using FluentValidation;

namespace UserManagement.Application.Commands.AuthenticateUser
{
    public class AuthorizeUserCommandValidator : AbstractValidator<AuthorizeUserCommand>
    {
        public AuthorizeUserCommandValidator(IServiceProvider serviceProvider)
        {
            RuleFor(x => x.Username)
               .NotEmpty()
               .WithMessage("{PropertyName} is required.");

            RuleFor(x => x.Password)
               .NotEmpty()
               .WithMessage("{PropertyName} is required.");
        }
    }
}
