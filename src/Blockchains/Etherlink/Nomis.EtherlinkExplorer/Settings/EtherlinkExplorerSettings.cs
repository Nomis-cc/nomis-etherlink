// ------------------------------------------------------------------------------------------------------
// <copyright file="EtherlinkExplorerSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Contracts;
using Nomis.Blockchain.Abstractions.Contracts.Settings;
using Nomis.Blockchain.Abstractions.Enums;
using Nomis.Blockchain.Abstractions.Settings;
using Nomis.NomisHolders.Interfaces.Enums;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Contracts.Requests;
using Nomis.Utils.Enums;

namespace Nomis.EtherlinkExplorer.Settings
{
    /// <summary>
    /// Etherlink Explorer settings.
    /// </summary>
    internal class EtherlinkExplorerSettings :
        IBlockchainSettings,
        IHasScoringBaseSettings,
        IGetFromCacheStatsSettings,
        IHttpClientLoggingSettings,
        IUseHistoricalMedianBalanceUSDSettings,
        IHasDiscountedMintFeeSettings,
        IDiscordSendingRequest
    {
        /// <inheritdoc />
        public IDictionary<ScoringChainType, ScoringBaseSettings> ScoringSettings { get; init; } = new Dictionary<ScoringChainType, ScoringBaseSettings>();

        /// <inheritdoc />
        public IDictionary<BlockchainKind, BlockchainDescriptor> BlockchainDescriptors { get; init; } = new Dictionary<BlockchainKind, BlockchainDescriptor>();

        /// <inheritdoc/>
        public bool GetFromCacheStatsIsEnabled { get; init; }

        /// <inheritdoc/>
        public TimeSpan GetFromCacheStatsTimeLimit { get; init; }

        /// <inheritdoc/>
        public bool UseHttpClientLogging { get; init; }

        /// <inheritdoc/>
        public IDictionary<ScoringCalculationModel, bool> UseHistoricalMedianBalanceUSD { get; init; } = new Dictionary<ScoringCalculationModel, bool>();

        /// <inheritdoc/>
        public decimal MedianBalancePrecision { get; init; }

        /// <inheritdoc/>
        public TimeSpan? MedianBalanceStartFrom { get; init; }

        /// <inheritdoc/>
        public int? MedianBalanceLastCount { get; init; }

        /// <inheritdoc/>
        public bool DiscountedMintFeeIsEnabled { get; init; }

        /// <inheritdoc/>
        public IDictionary<NomisHoldersScore, ulong?> DiscountedScores { get; init; } = new Dictionary<NomisHoldersScore, ulong?>();

        /// <inheritdoc/>
        public bool SendScoreToDiscord { get; set; }

        /// <summary>
        /// Image service version.
        /// </summary>
        public ImageServiceVersion ImageVersion { get; init; }
    }
}