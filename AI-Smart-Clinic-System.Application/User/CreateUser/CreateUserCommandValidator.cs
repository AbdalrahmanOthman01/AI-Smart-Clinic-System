using AI_Smart_Clinic_System.Application.User.CreateUser;
using FluentValidation;

namespace AI_Smart_Clinic_System.Application.User.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(20);


            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(20);


            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^\+?\d{7,15}$")
                .WithMessage("Phone number must be between 7 and 15 digits and may start with '+'.");


            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(30);
        }
    }
}
