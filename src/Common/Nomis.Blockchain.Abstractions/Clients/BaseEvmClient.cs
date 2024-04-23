// ------------------------------------------------------------------------------------------------------
// <copyright file="BaseEvmClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;

using Microsoft.Extensions.Logging;
using Nomis.Blockchain.Abstractions.Contracts.Models;
using Nomis.Blockchain.Abstractions.Contracts.Settings;
using Nomis.Blockchain.Abstractions.Enums;
using Nomis.Utils.Attributes.Logging;
using Nomis.Utils.Contracts.Common;
using Nomis.Utils.Exceptions;
using Serilog;
using SerilogTracing;

using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Nomis.Blockchain.Abstractions.Clients
{
    /// <inheritdoc cref="IBaseEvmClient"/>
    public class BaseEvmClient :
        IBaseEvmClient,
        IDisposable
    {
        private readonly IHasFetchLimits _settings;
        private readonly HttpClient _client;
        private readonly string? _appendedPath;
        private readonly string? _apiKey;
        private readonly object? _maskedApiKey;

        /// <summary>
        /// Initialize <see cref="BaseEvmClient"/>.
        /// </summary>
        /// <param name="settings"><see cref="IBlockchainSettings"/>.</param>
        /// <param name="blockchainSettings"><see cref="IBlockchainSettings"/>.</param>
        /// <param name="client"><see cref="HttpClient"/>.</param>
        /// <param name="logger"><see cref="ILogger{TCategoryName}"/>.</param>
        /// <param name="apiKey">API key.</param>
        /// <param name="appendedPath">Appended path for base URL.</param>
        public BaseEvmClient(
            IHasFetchLimits settings,
            IBlockchainSettings blockchainSettings,
            HttpClient client,
            ILogger logger,
            string? apiKey = null,
            string? appendedPath = null)
        {
            _settings = settings;

            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                _apiKey = apiKey;
                var maskedAttribute = new LogMaskedAttribute(showFirst: 3, preserveLength: true);
                _maskedApiKey = maskedAttribute.MaskValue(_apiKey);
                ulong chainId = blockchainSettings.BlockchainDescriptors.TryGetValue(BlockchainKind.Mainnet, out var mainnetValue)
                    ? mainnetValue.ChainId : blockchainSettings.BlockchainDescriptors[BlockchainKind.Testnet].ChainId;
                logger.LogDebug("Used {ApiKey} API key for {ChainId} chain ID.", _maskedApiKey, chainId);
            }

            _client = client;
            _appendedPath = appendedPath;
        }

        /// <inheritdoc/>
        public virtual async Task<BaseEvmAccount> GetBalanceAsync(string address, string tag = "latest")
        {
            using var activity = Log.Logger.StartActivity("Getting {Wallet} native balance with {ApiKey} API key.", address, _maskedApiKey);
            string request =
                $"/api?module=account&action=balance&address={address}&tag={tag}";
            if (!string.IsNullOrWhiteSpace(_apiKey))
            {
                request += $"&apiKey={_apiKey}";
            }

            if (!string.IsNullOrWhiteSpace(_appendedPath))
            {
                request = _appendedPath + request;
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<BaseEvmAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public virtual async Task<BaseEvmToken> GetTokenDataV2Async(string address)
        {
            using var activity = Log.Logger.StartActivity("Getting {Token} data with {ApiKey} API key.", address, _maskedApiKey);
            string request =
                $"/api/v2/tokens/{address}";
            if (!string.IsNullOrWhiteSpace(_apiKey))
            {
                request += $"&apiKey={_apiKey}";
            }

            if (!string.IsNullOrWhiteSpace(_appendedPath))
            {
                request = _appendedPath + request;
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<BaseEvmToken>().ConfigureAwait(false) ?? throw new CustomException("Can't get token data.");
        }

        /// <inheritdoc/>
        public virtual async Task<IList<BaseEvmTokenBalance>> GetTokenBalancesV2Async(string address)
        {
            using var activity = Log.Logger.StartActivity("Getting tokens balances for {Wallet} with {ApiKey} API key.", address, _maskedApiKey);
            string request =
                $"/api/v2/addresses/{address}/token-balances";
            if (!string.IsNullOrWhiteSpace(_apiKey))
            {
                request += $"&apiKey={_apiKey}";
            }

            if (!string.IsNullOrWhiteSpace(_appendedPath))
            {
                request = _appendedPath + request;
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<BaseEvmTokenBalance>>().ConfigureAwait(false) ?? throw new CustomException("Can't get account tokens balances.");
        }

        /// <inheritdoc/>
        public virtual async Task<BaseEvmAccount> GetTokenBalanceAsync(string address, string contractAddress)
        {
            using var activity = Log.Logger.StartActivity("Getting token {Contract} balance for {Wallet} with {ApiKey} API key.", contractAddress, address, _maskedApiKey);
            string request =
                $"/api?module=account&action=tokenbalance&address={address}&contractaddress={contractAddress}&tag=latest";
            if (!string.IsNullOrWhiteSpace(_apiKey))
            {
                request += $"&apiKey={_apiKey}";
            }

            if (!string.IsNullOrWhiteSpace(_appendedPath))
            {
                request = _appendedPath + request;
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BaseEvmAccount>().ConfigureAwait(false) ?? throw new CustomException("Can't get account token balance.");
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(
            string address,
            string? startBlockParameterName = "startblock",
            string? endBLockParameterName = "endblock")
            where TResult : IBaseEvmTransferList<TResultItem>
            where TResultItem : IBaseEvmTransfer
        {
            using var activity = Log.Logger.StartActivity("Getting {TypeName} for {Wallet} with {ApiKey} API key.", typeof(TResultItem).Name, address, _maskedApiKey);
            var result = new List<TResultItem>();
            string? startBlock = null;
            var transactionsData = await GetTransactionList<TResult>(address, null, startBlockParameterName, endBLockParameterName).ConfigureAwait(false);
            result.AddRange(transactionsData.Data ?? new List<TResultItem>());
            while (transactionsData.Data?.Count >= _settings.ItemsFetchLimitPerRequest && (_settings.TransactionsLimit == null || result.Count < _settings.TransactionsLimit))
            {
                if (transactionsData.Data.LastOrDefault()?.BlockNumber?.Equals(startBlock, StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    break;
                }

                startBlock = transactionsData.Data.LastOrDefault()?.BlockNumber;
                transactionsData = await GetTransactionList<TResult>(address, startBlock, startBlockParameterName, endBLockParameterName).ConfigureAwait(false);
                result.AddRange(transactionsData.Data ?? new List<TResultItem>());
            }

            return result;
        }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            _client.Dispose();
        }

        private async Task<TResult> GetTransactionList<TResult>(
            string address,
            string? startBlock = null,
            string? startBlockParameterName = "startblock",
            string? endBLockParameterName = "endblock")
        {
            string request =
                $"/api?module=account&address={address}&sort=asc";
            if (!string.IsNullOrWhiteSpace(_apiKey))
            {
                request += $"&apiKey={_apiKey}";
            }

            if (typeof(TResult) == typeof(BaseEvmNormalTransactions))
            {
                request = $"{request}&action=txlist";
            }
            else if (typeof(TResult) == typeof(BaseEvmInternalTransactions))
            {
                request = $"{request}&action=txlistinternal";
            }
            else if (typeof(TResult) == typeof(BaseEvmERC20TokenTransfers))
            {
                request = $"{request}&action=tokentx";
            }
            else if (typeof(TResult) == typeof(BaseEvmERC721TokenTransfers))
            {
                request = $"{request}&action=tokennfttx";
            }
            else if (typeof(TResult) == typeof(BaseEvmERC1155TokenTransfers))
            {
                request = $"{request}&action=token1155tx";
            }
            else
            {
                return default!;
            }

            request = !string.IsNullOrWhiteSpace(startBlock)
                ? $"{request}&{startBlockParameterName}={startBlock}"
                : $"{request}&{startBlockParameterName}=0";

            request = $"{request}&{endBLockParameterName}=999999999";

            if (!string.IsNullOrWhiteSpace(_appendedPath))
            {
                request = _appendedPath + request;
            }

            var response = await _client.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResult>().ConfigureAwait(false) ?? throw new CustomException("Can't get account transactions.");
        }
    }
}