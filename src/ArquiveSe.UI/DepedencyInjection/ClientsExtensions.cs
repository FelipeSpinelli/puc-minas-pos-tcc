using ArquiveSe.UI.Abstractions;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RestEase;

namespace ArquiveSe.UI.DepedencyInjection
{
    public static class ClientsExtensions
    {
        public static IServiceCollection AddClients(this IServiceCollection services)
        {
            services.AddHttpClient<IArquiveSeApi>(nameof(IArquiveSeApi));
            return services.AddSingleton(sp =>
            {
                var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(IArquiveSeApi));
                httpClient.BaseAddress = new Uri(sp.GetRequiredService<IConfiguration>()["ArquiveSeApiBaseAddress"].ToString());

                return new RestClient(httpClient)
                {
                    JsonSerializerSettings = new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new DefaultNamingStrategy()
                        }
                    }
                }.For<IArquiveSeApi>();
            });
        }
    }
}
