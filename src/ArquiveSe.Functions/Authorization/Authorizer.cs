using ArquiveSe.Functions.Authorization.Models;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ArquiveSe.Functions.Authorization
{
    internal static class Authorizer
    {
        private static string _issuer = "";
        private static string _audience = "";
        private static IConfigurationManager<OpenIdConnectConfiguration> _configurationManager;

        public static void Configure(string issuer, string audience)
        {
            _issuer = issuer;
            _audience = audience;

            var documentRetriever = new HttpDocumentRetriever { RequireHttps = _issuer.StartsWith("https://") };

            _configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                $"{_issuer}.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever(),
                documentRetriever
            );
        }

        public static async Task<AuthenticatedUser> ValidateTokenAsync(string value)
        {
            if (!(value?.StartsWith("Bearer") ?? false))
            {
                return null;
            }

            var config = await _configurationManager.GetConfigurationAsync(CancellationToken.None);

            var validationParameter = new TokenValidationParameters
            {
                RequireSignedTokens = true,
                ValidAudience = _audience,
                ValidateAudience = true,
                ValidIssuer = _issuer,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKeys = config.SigningKeys
            };

            ClaimsPrincipal principal = null;
            var tries = 0;

            while (principal == null && tries <= 1)
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    principal = handler.ValidateToken(value.Remove(0, 7), validationParameter, out var token);
                }
                catch (SecurityTokenSignatureKeyNotFoundException ex1)
                {
                    _configurationManager.RequestRefresh();
                    tries++;
                }
                catch (SecurityTokenException ex2)
                {
                    return null;
                }
            }

            return new(principal);
        }
    }
}