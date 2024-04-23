// ------------------------------------------------------------------------------------------------------
// <copyright file="CmsClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;
using System.Text.Json.Nodes;

using Nomis.CmsService.Interfaces.Enums;
using Nomis.CmsService.Interfaces.Responses;
using Nomis.Utils.Extensions;

namespace Nomis.CmsService
{
    /// <summary>
    /// CMS client.
    /// </summary>
    public class CmsClient
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Initialize <see cref="CmsClient"/>.
        /// </summary>
        /// <param name="client"><see cref="HttpClient"/>.</param>
        public CmsClient(
            HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Get referral data by owner wallet address.
        /// </summary>
        /// <param name="address">Owner wallet address.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        public async Task<NomisCmsResponse?> GetReferralCodesAsync(
            string address,
            CancellationToken cancellationToken = default)
        {
            var response = await _client.GetAsync($"/api/blockchain-event-changed-scores?pagination[pageSize]=1&filters[owner]={address}&fields=referrerCode&fields=referralCode", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<NomisCmsResponse>(cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Get referral actions count (mints and updates) by referrer code.
        /// </summary>
        /// <param name="referrer">Referrer code.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        public async Task<int> GetReferralActionsCountAsync(
            string referrer,
            CancellationToken cancellationToken = default)
        {
            var response = await _client.GetAsync($"/api/blockchain-event-changed-scores?filters[referrerCode]={referrer}&pagination[pageSize]=1&fields=id", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<NomisCmsResponse>(cancellationToken).ConfigureAwait(false);
            return result?.Meta?.Pagination?.Total ?? 0;
        }

        /// <summary>
        /// Get leaderboard data by owner wallet address.
        /// </summary>
        /// <param name="address">Owner wallet address.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        public async Task<NomisCmsResponse?> GetLeaderboardDataAsync(
            string address,
            CancellationToken cancellationToken = default)
        {
            var response = await _client.GetAsync($"/api/leaderboard-wallets?filters[owner][$eqi]={address}&filters[scoreName]=total", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<NomisCmsResponse>(cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Get EVM wallet address by social account.
        /// </summary>
        /// <param name="provider">CMS social account provider.</param>
        /// <param name="username">Social account username.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        public async Task<string?> GetEvmWalletAddressAsync(
            NomisCmsSocialAccountProvider provider,
            string username,
            CancellationToken cancellationToken = default)
        {
            var response = await _client.GetAsync($"/api/wallet-social-accounts?filters[provider]={provider.ToDescriptionString()}&filters[providerUsername]={username}&populate=wallet", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<JsonObject>(cancellationToken).ConfigureAwait(false);
            string? address = result?["data"]?[0]?["attributes"]?[NomisCmsDataAttribute.Wallet.ToDescriptionString()]?["data"]?["attributes"]?["address"]?.ToString();
            return address;
        }

        /// <summary>
        /// Get Solana wallet address by social account.
        /// </summary>
        /// <param name="provider">CMS social account provider.</param>
        /// <param name="username">Social account username.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        public async Task<string?> GetSolanaWalletAddressAsync(
            NomisCmsSocialAccountProvider provider,
            string username,
            CancellationToken cancellationToken = default)
        {
            var response = await _client.GetAsync($"/api/wallet-social-accounts?populate=wallet.solana_wallet&filters[providerUsername]={username}&filters[provider]={provider.ToDescriptionString()}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<JsonObject>(cancellationToken).ConfigureAwait(false);
            string? address = result?["data"]?[0]?["attributes"]?[NomisCmsDataAttribute.Wallet.ToDescriptionString()]?["data"]?["attributes"]?["solana_wallet"]?["data"]?["attributes"]?["address"]?.ToString();
            return address;
        }
    }
}