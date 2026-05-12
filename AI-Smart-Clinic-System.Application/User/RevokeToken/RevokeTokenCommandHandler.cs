using MediatR;
using AI_Smart_Clinic_System.Application.Interfaces;

namespace AI_Smart_Clinic_System.Application.User.RevokeToken
{
    internal sealed class RevokeTokenCommandHandler(IAuthService authService)
        : IRequestHandler<RevokeTokenCommand, Result>
    {
        public async Task<Result> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var result = await authService.RevokeRefreshTokenAsync(request.RefreshToken, cancellationToken);
            return result;
        }
    }
}
