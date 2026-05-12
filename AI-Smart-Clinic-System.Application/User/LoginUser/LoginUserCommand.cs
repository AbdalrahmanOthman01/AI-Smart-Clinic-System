using MediatR;
using AI_Smart_Clinic_System.Application.Dtos;

namespace AI_Smart_Clinic_System.Application.User.LoginUser
{
    public class LoginUserCommand : IRequest<Result<AuthResult>>
    {
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set;} = null!;
    }
}
