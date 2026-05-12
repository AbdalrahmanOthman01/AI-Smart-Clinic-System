using MediatR;
using AI_Smart_Clinic_System.Application.Dtos;

namespace AI_Smart_Clinic_System.Application.User.RefreshToken
{
    public class RefreshTokenCommand : IRequest<Result<AuthResult>>
    {
        public string RefreshToken { get; set; } = null!;
    }
}
