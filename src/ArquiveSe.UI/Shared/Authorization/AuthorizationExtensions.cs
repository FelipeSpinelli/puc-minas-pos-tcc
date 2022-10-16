using System.Security.Claims;
using System.Text;

namespace ArquiveSe.UI.Shared.Authorization
{
    public static class AuthorizationExtensions
    {
        public const string NAMEIDENTIFIER_CLAIM_TYPE = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public static string GetUserId(this ClaimsPrincipal user)
        {
            var claim = user?.Claims?.FirstOrDefault(x => x.Type.Equals(NAMEIDENTIFIER_CLAIM_TYPE));
            return claim?.Value ?? string.Empty;
        }

        public static string GetAccountId(this IHttpContextAccessor httpContextAccessor, string userId)
        {
            return httpContextAccessor?.HttpContext?.Request.Cookies[userId.GetAccountIdCookieKey()] ?? string.Empty;
        }

        public static string GetAccountIdCookieKey(this string userId)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userId}:acc"));
        }
    }
}
