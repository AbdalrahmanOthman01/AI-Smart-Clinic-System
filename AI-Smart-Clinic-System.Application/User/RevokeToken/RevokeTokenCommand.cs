using MediatR;

namespace AI_Smart_Clinic_System.Application.User.RevokeToken
{
    public class RevokeTokenCommand : IRequest<Result>
    {
        public string RefreshToken { get; set; } = null!;
    }
}
