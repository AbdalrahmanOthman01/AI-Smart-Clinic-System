using AI_Smart_Clinic_System.Application.Dtos;
using AI_Smart_Clinic_System.Application.User.CreateUser;

namespace AI_Smart_Clinic_System.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthResult>> RegisterAsync(CreateUserCommand command, CancellationToken cancellationToken = default);
        Task<Result<AuthResult>> GetTokenAsync(string phoneNumber, string password, CancellationToken cancellationToken = default);
        Task<Result<AuthResult>> GetRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

        Task<Result> RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}
