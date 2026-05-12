using FluentValidation;

namespace AI_Smart_Clinic_System.Application.User.LoginUser
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^\+?\d{7,15}$")
                .WithMessage("Phone number must be between 7 and 15 digits and may start with '+'.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
