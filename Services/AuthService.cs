using System.Security.Claims;
using vita_care.Repositories;

namespace vita_care.Services
{
    public interface IAuthService
    {
        Task<bool> HasRoleAsync(ClaimsPrincipal userPrincipal, string[] allowedRoles);
        string? GetUserEmail(ClaimsPrincipal userPrincipal);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string? GetUserEmail(ClaimsPrincipal userPrincipal)
        {
            return userPrincipal.FindFirst("email")?.Value ?? 
                   userPrincipal.FindFirst(ClaimTypes.Email)?.Value;
        }

        public async Task<bool> HasRoleAsync(ClaimsPrincipal userPrincipal, string[] allowedRoles)
        {
            var email = GetUserEmail(userPrincipal);
            if (string.IsNullOrEmpty(email)) return false;

            var user = await _userRepository.GetUserByEmailAsync(email, default);
            if (user == null) return false;

            // Check if any of the user's DB roles match the allowed roles
            return user.Roles.Any(r => allowedRoles.Contains(r, StringComparer.OrdinalIgnoreCase));
        }
    }
}
