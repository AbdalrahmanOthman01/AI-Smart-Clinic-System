using MediatR;
using AI_Smart_Clinic_System.Application.Dtos;
using AI_Smart_Clinic_System.Application.Interfaces;

namespace AI_Smart_Clinic_System.Application.User.CreateUser
{
    internal sealed class CreateUserCommandHandler(IAuthService authService)
        : IRequestHandler<CreateUserCommand, Result<AuthResult>>
    {
        public async Task<Result<AuthResult>> Handle(CreateUserCommand request, CancellationToken cancellationToken = default)
        {
            var result = await authService.RegisterAsync(request, cancellationToken);
            return result;
        }
    }
}
