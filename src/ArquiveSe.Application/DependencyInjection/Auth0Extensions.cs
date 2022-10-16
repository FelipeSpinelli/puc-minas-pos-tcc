using ArquiveSe.Application.Models;
using ArquiveSe.Application.UseCases;
using ArquiveSe.Application.UseCases.Abstractions;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArquiveSe.Application.DependencyInjection
{
    public static class Auth0Extensions
    {
        public static IServiceCollection AddAuth0(this IServiceCollection services)
        {
            return services
                .AddSingleton<CurrentAccessToken>()
                .AddSingleton<IAuthenticationApiClient>(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    var auth0Domain = configuration["Auth0Domain"].ToString();
                    return new AuthenticationApiClient(auth0Domain);
                })
                .AddSingleton<IManagementConnection, HttpClientManagementConnection>()
                .AddScoped<IManagementApiClient>(sp => 
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    var auth0Domain = configuration["Auth0Domain"].ToString(); 
                    
                    var managementConnection = sp.GetRequiredService<IManagementConnection>();
                    var currentAccessToken = sp.GetRequiredService<CurrentAccessToken>();
                    var authenticationApiClient = new AuthenticationApiClient(auth0Domain);

                    currentAccessToken.UpdateIfNecessary(async (isExpired, accessToken) =>
                    {
                        if (!isExpired)
                        {
                            return accessToken;
                        }

                        return await authenticationApiClient.GetTokenAsync(new ClientCredentialsTokenRequest
                        {
                            Audience = configuration["Auth0Audience"],
                            ClientId = configuration["Auth0ClientId"],
                            ClientSecret = configuration["Auth0ClientSecret"]
                        });
                    }).GetAwaiter().GetResult();

                    return new ManagementApiClient(currentAccessToken.AccessToken, auth0Domain, managementConnection);
                })
                .AddScoped<IUploadFileUseCase, UploadFileUseCase>()
                .AddScoped<ICreateAccountUseCase, CreateAccountUseCase>();
        }
}
}
