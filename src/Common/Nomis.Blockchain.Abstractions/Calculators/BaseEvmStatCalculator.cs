﻿// ------------------------------------------------------------------------------------------------------
// <copyright file="BaseEvmStatCalculator.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using Nomis.Blockchain.Abstractions.Contracts.Data;
using Nomis.Blockchain.Abstractions.Contracts.Models;
using Nomis.Blockchain.Abstractions.Stats;
using Nomis.Utils.Contracts;
using Nomis.Utils.Contracts.Calculators;
using Nomis.Utils.Contracts.Stats;
using Nomis.Utils.Extensions;

namespace Nomis.Blockchain.Abstractions.Calculators
{
    /// <summary>
    /// Base EVM stat calculator.
    /// </summary>
    public class BaseEvmStatCalculator<TWalletStats, TTransactionIntervalData> :
        IWalletCommonStatsCalculator<TWalletStats, TTransactionIntervalData>,
        IWalletBalanceStatsCalculator<TWalletStats, TTransactionIntervalData>,
        IWalletTransactionStatsCalculator<TWalletStats, TTransactionIntervalData>,
        IWalletTokenStatsCalculator<TWalletStats, TTransactionIntervalData>,
        IWalletContractStatsCalculator<TWalletStats, TTransactionIntervalData>
        where TWalletStats : class, IWalletCommonStats<TTransactionIntervalData>, IWalletBalanceStats, IWalletTransactionStats, IWalletTokenStats, IWalletContractStats, new()
        where TTransactionIntervalData : class, ITransactionIntervalData, new()
    {
        // ReSharper disable once InconsistentNaming
        private readonly decimal _tokenUSDPrice;
        private readonly string _address;
        private readonly IEnumerable<BaseEvmNormalTransaction> _transactions;
        private readonly IEnumerable<BaseEvmERC20TokenTransfer> _erc20TokenTransfers;
        private readonly IEnumerable<TokenDataBalance>? _tokenDataBalances;
        private readonly Func<decimal, decimal> _toNativeFunc;

        /// <inheritdoc />
        public int WalletAge => _transactions.Any()
            ? IWalletStatsCalculator.GetWalletAge(_transactions.Select(x => x.Timestamp!.ToDateTime()))
            : 0;

        /// <inheritdoc />
        public DateTime ScoredAt { get; set; } = DateTime.UtcNow;

        /// <inheritdoc />
        public IList<TTransactionIntervalData> TurnoverIntervals
        {
            get
            {
                var turnoverIntervalsDataList =
                    _transactions.Select(x => new TurnoverIntervalsData(
                        x.Timestamp!.ToDateTime(),
                        BigInteger.TryParse(x.Value, out var value) ? value : 0,
                        x.From?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true));
                return IWalletStatsCalculator<TTransactionIntervalData>
                    .GetTurnoverIntervals(_tokenUSDPrice, turnoverIntervalsDataList, _transactions.Any() ? _transactions.Min(x => x.Timestamp!.ToDateTime()) : DateTime.MinValue).ToList();
            }
        }

        /// <inheritdoc />
        public decimal NativeBalance { get; }

        /// <inheritdoc />
        public decimal HistoricalMedianBalanceUSD { get; }

        /// <inheritdoc />
        public decimal NativeBalanceUSD { get; }

        /// <inheritdoc />
        public decimal BalanceChangeInLastMonth =>
            IWalletStatsCalculator<TTransactionIntervalData>.GetBalanceChangeInLastMonth(TurnoverIntervals);

        /// <inheritdoc />
        public decimal BalanceChangeInLastYear =>
            IWalletStatsCalculator<TTransactionIntervalData>.GetBalanceChangeInLastYear(TurnoverIntervals);

        /// <inheritdoc />
        public decimal WalletTurnover =>
            _transactions.Sum(x => decimal.TryParse(x.Value, out decimal value) ? _toNativeFunc(value) : 0);

        /// <inheritdoc />
        public decimal WalletTurnoverUSD => NativeBalance != 0 ? WalletTurnover * NativeBalanceUSD / NativeBalance : 0;

        /// <inheritdoc />
        public IEnumerable<TokenDataBalance>? TokenBalances => _tokenDataBalances?.Any() == true ? _tokenDataBalances : null;

        /// <inheritdoc />
        public int TokensHolding => _erc20TokenTransfers.Select(x => x.TokenSymbol).Distinct().Count();

        /// <inheritdoc />
        public int DeployedContracts => _transactions.Count(x => !string.IsNullOrWhiteSpace(x.ContractAddress));

        /// <inheritdoc />
        public IEnumerable<TransferTokenDataBalance>? TransferTokens { get; }

        /// <summary>
        /// Initialize <see cref="BaseEvmStatCalculator{TWalletStats,TTransactionIntervalData}"/>.
        /// </summary>
        /// <param name="address">Wallet address.</param>
        /// <param name="balance">Wallet native balance.</param>
        /// <param name="usdBalance">Wallet native balance in USD.</param>
        /// <param name="medianUsdBalance">Median native balance in USD.</param>
        /// <param name="transactions">Normal transaction list.</param>
        /// <param name="erc20TokenTransfers">ERC-20 token transfers.</param>
        /// <param name="tokenDataBalances">Token data balances.</param>
        /// <param name="toNativeFunc">Function for converting wei value to native.</param>
        /// <param name="transferTokenDataBalances">Transfer tokens data balances.</param>
        public BaseEvmStatCalculator(
            string address,
            decimal balance,
            decimal usdBalance,
            decimal medianUsdBalance,
            IEnumerable<BaseEvmNormalTransaction> transactions,
            IEnumerable<BaseEvmERC20TokenTransfer> erc20TokenTransfers,
            IEnumerable<TokenDataBalance>? tokenDataBalances,
            Func<decimal, decimal> toNativeFunc,
            IEnumerable<TransferTokenDataBalance>? transferTokenDataBalances = null)
        {
            _tokenUSDPrice = balance > 0 ? usdBalance / toNativeFunc(balance) : 0;
            _address = address;
            NativeBalance = toNativeFunc(balance);
            NativeBalanceUSD = usdBalance;
            HistoricalMedianBalanceUSD = medianUsdBalance;
            _transactions = transactions;
            _erc20TokenTransfers = erc20TokenTransfers;
            _tokenDataBalances = tokenDataBalances;
            _toNativeFunc = toNativeFunc;
            TransferTokens = transferTokenDataBalances;
        }

        /// <inheritdoc />
        public TWalletStats Stats()
        {
            return (this as IWalletStatsCalculator<TWalletStats, TTransactionIntervalData>).ApplyCalculators();
        }

        /// <inheritdoc />
        IWalletTransactionStats IWalletTransactionStatsCalculator<TWalletStats, TTransactionIntervalData>.Stats()
        {
            if (!_transactions.Any())
            {
                return new TWalletStats
                {
                    NoData = true
                };
            }

            var intervals = IWalletStatsCalculator
                .GetTransactionsIntervals(_transactions.Select(x => x.Timestamp!.ToDateTime())).ToList();
            if (intervals.Count == 0)
            {
                return new TWalletStats
                {
                    NoData = true
                };
            }

            var now = DateTime.UtcNow;
            var monthAgo = now.AddMonths(-1);
            var yearAgo = now.AddYears(-1);

            return new TWalletStats
            {
                TotalTransactions = _transactions.Count(),
                TotalRejectedTransactions = _transactions.Count(t => string.Equals(t.IsError, "1", StringComparison.OrdinalIgnoreCase)),
                MinTransactionTime = intervals.Min(),
                MaxTransactionTime = intervals.Max(),
                AverageTransactionTime = intervals.Average(),
                LastMonthTransactions = _transactions.Count(x => x.Timestamp!.ToDateTime() > monthAgo),
                LastYearTransactions = _transactions.Count(x => x.Timestamp!.ToDateTime() > yearAgo),
                TimeSinceTheLastTransaction = (int)((now - _transactions.OrderBy(x => x.Timestamp).Last().Timestamp!.ToDateTime()).TotalDays / 30)
            };
        }
    }
}