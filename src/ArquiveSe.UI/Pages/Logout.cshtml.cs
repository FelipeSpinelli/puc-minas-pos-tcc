using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArquiveSe.UI.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly IAuthorizationService _authorizationService;

        public LogoutModel(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [Authorize]
        public async Task OnGet()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                 .WithRedirectUri("/")
                 .Build();

            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var user = _authorizationService.
        }
    }
}