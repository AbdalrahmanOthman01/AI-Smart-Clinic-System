using System.Security.Claims;
using AI_Smart_Clinic_System.Application.Interfaces;

namespace AI_Smart_Clinic_System.API.Services
{
    public class UserService(IHttpContextAccessor _context) : IUserService
    {
        public string? GetCurrentUserId()
        {
            return _context.HttpContext!.User.FindFirstValue("uid");
        }
    }
}
