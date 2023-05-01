using ArquiveSe.App.Services.ArquiveSeApi;
using Microsoft.Extensions.Options;

namespace ArquiveSe.App.Services
{
    public static class ServicesDependencyInjectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            return services
                .Configure<ApiSettings>(configuration.GetSection(ApiSettings.SECTION))
                .AddScoped<IArquiveSeApiClient>(sp =>
                {
                    var apiSettings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
                    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(IArquiveSeApiClient));
                    httpClient.BaseAddress = apiSettings.BaseUrl;

                    return new ArquiveSeApiClient(httpClient);
                });
        }
    }
}
