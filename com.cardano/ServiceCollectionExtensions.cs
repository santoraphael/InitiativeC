using Microsoft.Extensions.DependencyInjection;
using Blockfrost.Api.Extensions;

namespace com.cardano
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCardanoServices(this IServiceCollection services, string blockfrostApiKey)
        {
            services.AddBlockfrost("mainnet", blockfrostApiKey);
            // Registre outros serviços aqui, se necessário
            return services;
        }
    }
}
