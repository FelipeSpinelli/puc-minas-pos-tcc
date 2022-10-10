using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ArquiveSe.Domain.DependencyInjection
{
    public static class DomainExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            return services
                .AddMediatR(typeof(DomainExtensions).Assembly);
        }
    }
}
