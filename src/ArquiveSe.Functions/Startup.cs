using ArquiveSe.Application.DependencyInjection;
using ArquiveSe.Domain.DependencyInjection;
using ArquiveSe.Functions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace ArquiveSe.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddDomain()
                .AddUseCases()
                .AddDataAccess()
                .AddBlobStorage();
        }
    }
}
