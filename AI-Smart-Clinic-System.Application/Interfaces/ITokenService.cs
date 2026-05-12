using System.Security.Claims;
using AI_Smart_Clinic_System.Domain.Entities;

namespace AI_Smart_Clinic_System.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateJwtToken(ApplicationUser user, IList<Claim> userClaims);
        public RefreshToken GenerateRefreshToken();
    }
}
