using MediatR;
using AI_Smart_Clinic_System.Application.Dtos;
using AI_Smart_Clinic_System.Application.Interfaces;

namespace AI_Smart_Clinic_System.Application.User.LoginUser
{
    internal sealed class LoginUserCommandHandler(IAuthService authService)
        : IRequestHandler<LoginUserCommand, Result<AuthResult>>
    {
        public async Task<Result<AuthResult>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = await authService.GetTokenAsync(request.PhoneNumber, request.Password, cancellationToken);
            return result;
        }
    }
}
