// ------------------------------------------------------------------------------------------------------
// <copyright file="EtherlinkStatCalculator.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Calculators;
using Nomis.Blockchain.Abstractions.Contracts.Data;
using Nomis.Blockchain.Abstractions.Contracts.Models;
using Nomis.EtherlinkExplorer.Interfaces.Extensions;
using Nomis.EtherlinkExplorer.Interfaces.Models;

namespace Nomis.EtherlinkExplorer.Calculators
{
    /// <summary>
    /// Etherlink wallet stats calculator.
    /// </summary>
    internal sealed class EtherlinkStatCalculator :
        BaseEvmStatCalculator<EtherlinkWalletStats, EtherlinkTransactionIntervalData>
    {
        public EtherlinkStatCalculator(
            string address,
            decimal balance,
            decimal usdBalance,
            decimal medianUsdBalance,
            IEnumerable<BaseEvmNormalTransaction> transactions,
            IEnumerable<BaseEvmERC20TokenTransfer> erc20TokenTransfers,
            IEnumerable<TokenDataBalance>? tokenDataBalances,
            IEnumerable<TransferTokenDataBalance>? transferTokenDataBalances = null)
            : base(address, balance, usdBalance, medianUsdBalance, transactions, erc20TokenTransfers, tokenDataBalances, value => value.ToXtz(), transferTokenDataBalances)
        {
        }
    }
}