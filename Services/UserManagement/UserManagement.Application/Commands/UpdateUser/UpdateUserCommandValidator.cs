using FluentValidation;

namespace UserManagement.Application.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator(IServiceProvider serviceProvider)
        {
            RuleFor(x => x.UserId)
               .NotEmpty()
               .WithMessage("{PropertyName} is required.");

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
