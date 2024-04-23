// ------------------------------------------------------------------------------------------------------
// <copyright file="CmsService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nomis.CmsService.Interfaces;
using Nomis.CmsService.Interfaces.Enums;
using Nomis.CmsService.Settings;
using Nomis.Utils.Extensions;
using Nomis.Utils.Wrapper;

namespace Nomis.CmsService
{
    /// <inheritdoc cref="ICmsService" />
    public class CmsService :
        ICmsService
    {
        private readonly CmsClient _client;
        private readonly ILogger<CmsService> _logger;
        private readonly CmsServiceSettings _settings;

        /// <summary>
        /// Initialize <see cref="CmsService"/>.
        /// </summary>
        /// <param name="settings"><see cref="CmsServiceSettings"/>.</param>
        /// <param name="client"><see cref="CmsClient"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public CmsService(
            IOptions<CmsServiceSettings> settings,
            CmsClient client,
            ILogger<CmsService> logger)
        {
            _client = client;
            _logger = logger;
            _settings = settings.Value;
        }

        /// <inheritdoc />
        public async Task<Result<IDictionary<string, string?>>> AccountDataBySocialAccountAsync(
            NomisCmsSocialAccountProvider provider,
            string username,
            CancellationToken cancellationToken)
        {
            var result = new Dictionary<string, string?>();

            try
            {
                // get wallet address by social account
                string? walletAddress = await _client.GetEvmWalletAddressAsync(provider, username, cancellationToken).ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(walletAddress))
                {
                    return await Result<IDictionary<string, string?>>.FailAsync("There is no EVM wallet for this social account.").ConfigureAwait(false);
                }

                result.Add(NomisCmsDataAttribute.Wallet.ToDescriptionString(), walletAddress);

                // get Solana wallet address
                string? solanaWalletAddress = await _client.GetSolanaWalletAddressAsync(provider, username, cancellationToken).ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(solanaWalletAddress))
                {
                    _logger.LogWarning("There is no Solana wallet for {WalletAddress} by social account provider {Provider} and username {Username}.", walletAddress, provider.ToString(), username);
                }
                else
                {
                    result.Add(NomisCmsDataAttribute.SolanaWallet.ToDescriptionString(), solanaWalletAddress);
                }

                // get referral data
                var referralData = await _client.GetReferralCodesAsync(walletAddress, cancellationToken).ConfigureAwait(false);
                if (referralData?.Data.Any() != true)
                {
                    _logger.LogWarning("There is no referral data for {WalletAddress} by social account provider {Provider} and username {Username}.", walletAddress, provider.ToString(), username);
                }
                else
                {
                    result.Add(NomisCmsDataAttribute.ReferrerCode.ToDescriptionString(), referralData.Data.FirstOrDefault()?.Attributes[NomisCmsDataAttribute.ReferrerCode.ToDescriptionString()]?.ToString());
                    result.Add(NomisCmsDataAttribute.ReferralCode.ToDescriptionString(), referralData.Data.FirstOrDefault()?.Attributes[NomisCmsDataAttribute.ReferralCode.ToDescriptionString()]?.ToString());
                }

                // get referrals count
                int referralCount = await _client.GetReferralActionsCountAsync(walletAddress, cancellationToken).ConfigureAwait(false);
                result.Add(NomisCmsDataAttribute.ReferralCount.ToDescriptionString(), referralCount.ToString());

                // get leaderboard data
                var leaderboardData = await _client.GetLeaderboardDataAsync(walletAddress, cancellationToken).ConfigureAwait(false);
                if (leaderboardData?.Data.Any() != true)
                {
                    _logger.LogWarning("There is no leaderboard data for {WalletAddress} by social account provider {Provider} and username {Username}.", walletAddress, provider.ToString(), username);
                }
                else
                {
                    result.Add(NomisCmsDataAttribute.Points.ToDescriptionString(), leaderboardData.Data.FirstOrDefault()?.Attributes[NomisCmsDataAttribute.Points.ToDescriptionString()]?.ToString());
                    result.Add(NomisCmsDataAttribute.Score.ToDescriptionString(), leaderboardData.Data.FirstOrDefault()?.Attributes[NomisCmsDataAttribute.Score.ToDescriptionString()]?.ToString());
                    result.Add(NomisCmsDataAttribute.Rank.ToDescriptionString(), leaderboardData.Data.FirstOrDefault()?.Attributes[NomisCmsDataAttribute.Rank.ToDescriptionString()]?.ToString());
                }

                return await Result<IDictionary<string, string?>>.SuccessAsync(result, "Successfully got account data by social account.").ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "There is an error in {Method}", nameof(AccountDataBySocialAccountAsync));
                return await Result<IDictionary<string, string?>>.FailAsync("There is an error occurs while getting account data by social account.").ConfigureAwait(false);
            }
        }
    }
}