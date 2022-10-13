using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace ArquiveSe.UI.Authorization
{
    public class RedirectUserWithoutAccount
    {
        public static bool Validate(ClaimsPrincipal user, NavigationManager navigationManager)
        {
            if (user?.HasClaim(claim => claim.Type.Equals("AccountId")) ?? false)
            {
                return true;
            }

            navigationManager.NavigateTo("/createaccount", true);
            return false;
        }
    }
}
