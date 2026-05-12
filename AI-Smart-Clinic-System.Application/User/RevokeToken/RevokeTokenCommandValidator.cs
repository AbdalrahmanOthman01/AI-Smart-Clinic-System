using FluentValidation;

namespace AI_Smart_Clinic_System.Application.User.RevokeToken
{
    public class RevokeTokenCommandValidator : AbstractValidator<RevokeTokenCommand>
    {
        public RevokeTokenCommandValidator()
        {
            RuleFor(x=>x.RefreshToken)
                .NotEmpty();
        }
    }
}
