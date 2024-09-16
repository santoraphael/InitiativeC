using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardanoSharp.Blockfrost.Sdk;
using CardanoSharp.Blockfrost.Sdk.Contracts;
using CardanoSharp.Wallet;
using CardanoSharp.Wallet.Encoding;
using CardanoSharp.Wallet.Enums;
using CardanoSharp.Wallet.Extensions.Models;
using CardanoSharp.Wallet.Models.Addresses;
using Refit;

namespace com.cardano
{
    public class BlockfrostServices
    {
        private readonly IAssetsClient _assetsClient;
        private readonly IEpochsClient _epochsClient;
        private readonly IAddressesClient _addressesClient;
        private readonly IAccountClient _accountsClient;

        public BlockfrostServices(IAssetsClient assetsClient
                                    , IEpochsClient epochsClient
                                    , IAddressesClient addressesClient
                                    , IAccountClient accountClient)
        {
            _assetsClient = assetsClient;
            _epochsClient = epochsClient;
            _addressesClient = addressesClient;
            _accountsClient = accountClient;
        }


        public async Task<string> GetStakeAddress(string walletAddress, NetworkType networkType = NetworkType.Mainnet)
        {
            try
            {
                var addressBytes = Bech32.Decode(walletAddress, out var witVer, out var hrp);
                var address = new Address(addressBytes);

                var baseAddress =  address.GetStakeAddress();

                return baseAddress.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao processar o endereço: {ex.Message}");
            }
        }

        public async Task<List<string>> GetAllAddressesByStakeAddress(string stakeAddress)
        {
            try
            {
                var allAddresses = new List<string>();
                int page = 1;
                int count = 100;
                bool hasMore = true;

                var addresses = await _accountsClient.GetAccountAssociatedAddresses(stakeAddress, count, page);

                if (addresses != null && addresses.IsSuccessStatusCode)
                {
                    foreach (var item in addresses.Content)
                    {
                        allAddresses.Add(item.Address);
                    }

                    page++;
                }


                return allAddresses;
            }
            catch (ApiException apiEx)
            {
                throw new Exception($"Erro da API Blockfrost: {apiEx.StatusCode} - {apiEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter os endereços: {ex.Message}");
            }
        }
    }
}
