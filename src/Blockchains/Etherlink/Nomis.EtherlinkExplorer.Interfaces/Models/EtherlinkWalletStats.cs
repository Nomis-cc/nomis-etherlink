// ------------------------------------------------------------------------------------------------------
// <copyright file="EtherlinkWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.EtherlinkExplorer.Interfaces.Models
{
    /// <summary>
    /// Etherlink wallet stats.
    /// </summary>
    public sealed class EtherlinkWalletStats :
        BaseEvmWalletStats<EtherlinkTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "XTZ";
    }
}