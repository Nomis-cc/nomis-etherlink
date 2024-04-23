﻿// ------------------------------------------------------------------------------------------------------
// <copyright file="EtherlinkExplorerTestnetClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Nomis.Blockchain.Abstractions.Clients;
using Nomis.Blockchain.Abstractions.Settings;
using Nomis.EtherlinkExplorer.Interfaces;
using Nomis.EtherlinkExplorer.Settings;
using Nomis.Utils.Contracts;

namespace Nomis.EtherlinkExplorer
{
    /// <inheritdoc cref="IEtherlinkExplorerTestnetClient"/>
    internal sealed class EtherlinkExplorerTestnetClient :
        BaseEvmClient,
        IEtherlinkExplorerTestnetClient
    {
        /// <summary>
        /// Initialize <see cref="EtherlinkExplorerTestnetClient"/>.
        /// </summary>
        /// <param name="settings"><see cref="EtherlinkExplorerSettings"/>.</param>
        /// <param name="scoringBaseSettings"><see cref="ScoringBaseSettings"/>.</param>
        /// <param name="apiKeysPool"><see cref="IValuePool{TService,TValue}"/>.</param>
        /// <param name="client"><see cref="HttpClient"/>.</param>
        /// <param name="logger"><see cref="ILogger{TCategoryName}"/>.</param>
        /// <param name="poolIndex">Pool index.</param>
        public EtherlinkExplorerTestnetClient(
            EtherlinkExplorerSettings settings,
            ScoringBaseSettings scoringBaseSettings,
            IValuePool<EtherlinkExplorerService, string> apiKeysPool,
            HttpClient client,
            ILogger<EtherlinkExplorerTestnetClient> logger,
            int poolIndex = 0)
            : base(scoringBaseSettings, settings, client, logger, apiKeysPool.GetNextValue(poolIndex), scoringBaseSettings.AppendedPath)
        {
        }

        /// <summary>
        /// Initialize <see cref="EtherlinkExplorerTestnetClient"/>.
        /// </summary>
        /// <param name="settings"><see cref="EtherlinkExplorerSettings"/>.</param>
        /// <param name="scoringBaseSettings"><see cref="ScoringBaseSettings"/>.</param>
        /// <param name="apiKeysPool"><see cref="IValuePool{TValue}"/>.</param>
        /// <param name="client"><see cref="HttpClient"/>.</param>
        /// <param name="logger"><see cref="ILogger{TCategoryName}"/>.</param>
        /// <param name="poolIndex">Pool index.</param>
        public EtherlinkExplorerTestnetClient(
            EtherlinkExplorerSettings settings,
            ScoringBaseSettings scoringBaseSettings,
            IValuePool<string> apiKeysPool,
            HttpClient client,
            ILogger<EtherlinkExplorerTestnetClient> logger,
            int poolIndex = 0)
            : base(scoringBaseSettings, settings, client, logger, apiKeysPool.GetNextValue(poolIndex), scoringBaseSettings.AppendedPath)
        {
        }
    }
}