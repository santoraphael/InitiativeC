using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Blockfrost.Api;
using Blockfrost.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using Blockfrost.Api.Extensions;

namespace com.cardano
{
    public class BlockfrostService
    {
        private readonly IAddressesService _addressesService;
        private readonly IAccountsService _accountsService;

        public BlockfrostService()
        {
            var projectId = "mainnet9Zmdv75dfVyFF0DzGoLVbQXoqX16yaOp";
            var network = "mainnet"; // ou "testnet"

            // Configurar os serviços
            var services = new ServiceCollection();

            // Utilize esta sobrecarga de AddBlockfrost
            services.AddBlockfrost(network, projectId);

            // Construir o provedor de serviços
            var serviceProvider = services.BuildServiceProvider();

            // Obter os serviços necessários
            _addressesService = serviceProvider.GetRequiredService<IAddressesService>();
            _accountsService = serviceProvider.GetRequiredService<IAccountsService>();
        }

        public async Task<string> GetStakeAddressFromWalletAddress(string walletAddress)
        {

            var BaseUrl = _accountsService.BaseUrl;
            var Network = _accountsService.Network;
            var Health = _accountsService.Health;
            var Metrics = _accountsService.Metrics;

            var addressContent = await _addressesService.GetTotalAsync(walletAddress);
            return addressContent.Address;
        }

        public async Task<IList<string>> GetWalletAddressesFromStakeAddress(string stakeAddress, int? count, int? page)
        {
            var addresses = await _accountsService.GetAddressesAsync(stakeAddress, count, page, ESortOrder.Asc);
            return addresses.Select(a => a.Address).ToList();
        }

    }
}
