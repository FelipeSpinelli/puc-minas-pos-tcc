using ArquiveSe.Application.DependencyInjection;
using ArquiveSe.Domain.DependencyInjection;
using ArquiveSe.Functions;
using ArquiveSe.Functions.Authorization;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Startup))]
namespace ArquiveSe.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            Authorizer.Configure(
                Environment.GetEnvironmentVariable("Auth0Issuer"),
                Environment.GetEnvironmentVariable("Auth0Audience"));

            builder.Services
                .AddDomain()
                .AddUseCases()
                .AddDataAccess()
                .AddBlobStorage();
        }
    }
}
