using AI_Smart_Clinic_System.Application.Dtos;
using MediatR;

namespace AI_Smart_Clinic_System.Application.User.CreateUser
{
    public class CreateUserCommand : IRequest<Result<AuthResult>>
    {
        //command params
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
