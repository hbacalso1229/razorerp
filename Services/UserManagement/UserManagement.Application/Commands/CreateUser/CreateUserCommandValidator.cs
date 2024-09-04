using FluentValidation;

namespace UserManagement.Application.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator(IServiceProvider serviceProvider)
        {
            //TODO Validate email
            RuleFor(x => x.Email)
               .NotEmpty()
               .WithMessage("{PropertyName} is required.");

            RuleFor(x => x.UserName)
               .NotEmpty()
               .WithMessage("{PropertyName} is required.");

            RuleFor(x => x.Password)
               .NotEmpty()
               .WithMessage("{PropertyName} is required.");

            RuleFor(x => x.CompanyId)
               .NotEmpty()
               .WithMessage("{PropertyName} is required.");

            RuleFor(x => x.Role)
               .NotEmpty()
               .WithMessage("{PropertyName} is required.");
        }
    }
}
