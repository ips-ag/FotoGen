using System.Security.Claims;

namespace FotoGen
{
    public interface IUserContext
    {
        string? UserId { get; }
        string? UserName { get; }
        string? Email { get; }
        string? GetClaimValue(string claimType);
    }

    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string? UserName => GetClaimValue(ClaimTypes.GivenName);
        public string? Email => GetClaimValue(ClaimTypes.Email);
        public string? UserId => GetClaimValue("oid") ?? GetClaimValue("http://schemas.microsoft.com/identity/claims/objectidentifier");
        public string? GetClaimValue(string claimType)
        {
            return _httpContextAccessor.HttpContext?.User?
                .Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }
    }
}
