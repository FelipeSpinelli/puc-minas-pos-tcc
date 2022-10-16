using ArquiveSe.Application.UseCases;
using ArquiveSe.Application.UseCases.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ArquiveSe.Application.DependencyInjection
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            return services
                .AddScoped<IUploadFileUseCase, UploadFileUseCase>()
                .AddScoped<ICreateAccountUseCase, CreateAccountUseCase>();
        }
    }
}
