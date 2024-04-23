// ------------------------------------------------------------------------------------------------------
// <copyright file="EtherlinkExplorerService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net;
using System.Text.Json;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nethereum.Util;
using Nomis.Blockchain.Abstractions;
using Nomis.Blockchain.Abstractions.Contracts;
using Nomis.Blockchain.Abstractions.Contracts.Data;
using Nomis.Blockchain.Abstractions.Contracts.Models;
using Nomis.Blockchain.Abstractions.Enums;
using Nomis.Blockchain.Abstractions.Extensions;
using Nomis.Blockchain.Abstractions.Requests;
using Nomis.Blockchain.Abstractions.Settings;
using Nomis.CacheProviderService.Interfaces;
using Nomis.DefiLlama.Interfaces;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.DexProviderService.Interfaces;
using Nomis.DexProviderService.Interfaces.Extensions;
using Nomis.Domain.Scoring.Entities;
using Nomis.EtherlinkExplorer.Calculators;
using Nomis.EtherlinkExplorer.Interfaces;
using Nomis.EtherlinkExplorer.Interfaces.Extensions;
using Nomis.EtherlinkExplorer.Interfaces.Models;
using Nomis.EtherlinkExplorer.Interfaces.Requests;
using Nomis.EtherlinkExplorer.Settings;
using Nomis.IPFS.Interfaces;
using Nomis.NomisHolders.Interfaces;
using Nomis.NomisHolders.Interfaces.Enums;
using Nomis.PolygonId.Interfaces;
using Nomis.ReferralService.Interfaces;
using Nomis.ReferralService.Interfaces.Extensions;
using Nomis.ScoringService.Interfaces;
using Nomis.SoulboundTokenService.Interfaces;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.SoulboundTokenService.Interfaces.Extensions;
using Nomis.Utils.Contracts;
using Nomis.Utils.Contracts.NFT;
using Nomis.Utils.Contracts.Requests;
using Nomis.Utils.Contracts.Services;
using Nomis.Utils.Contracts.Stats;
using Nomis.Utils.Enums;
using Nomis.Utils.Exceptions;
using Nomis.Utils.Extensions;
using Nomis.Utils.Wrapper;

namespace Nomis.EtherlinkExplorer
{
    /// <inheritdoc cref="IEtherlinkScoringService"/>
    internal sealed class EtherlinkExplorerService :
        BlockchainDescriptor,
        IEtherlinkScoringService,
        ITransientService
    {
        private readonly EtherlinkExplorerSettings _settings;
        private readonly BlacklistSettings _blacklistSettings;
        private readonly IEtherlinkExplorerClient _client;
        private readonly IEtherlinkExplorerTestnetClient _testnetClient;
        private readonly IScoringService _scoringService;
        private readonly IReferralService _referralService;
        private readonly IEvmScoreSoulboundTokenService _soulboundTokenService;
        private readonly IDexProviderService _dexProviderService;
        private readonly IDefiLlamaService _defiLlamaService;
        private readonly IIPFSService _ipfsService;
        private readonly IPolygonIdService _polygonIdService;
        private readonly ICacheProviderService _cacheProviderService;
        private readonly INomisHoldersService _nomisHoldersService;

        /// <summary>
        /// Initialize <see cref="EtherlinkExplorerService"/>.
        /// </summary>
        /// <param name="blacklistSettings"><see cref="BlacklistSettings"/>.</param>
        /// <param name="settings"><see cref="EtherlinkExplorerSettings"/>.</param>
        /// <param name="client"><see cref="IEtherlinkExplorerClient"/>.</param>
        /// <param name="testnetClient"><see cref="IEtherlinkExplorerTestnetClient"/>.</param>
        /// <param name="scoringService"><see cref="IScoringService"/>.</param>
        /// <param name="referralService"><see cref="IReferralService"/>.</param>
        /// <param name="soulboundTokenService"><see cref="IEvmScoreSoulboundTokenService"/>.</param>
        /// <param name="dexProviderService"><see cref="IDexProviderService"/>.</param>
        /// <param name="defiLlamaService"><see cref="IDefiLlamaService"/>.</param>
        /// <param name="ipfsService"><see cref="IIPFSService"/>.</param>
        /// <param name="polygonIdService"><see cref="IPolygonIdService"/>.</param>
        /// <param name="cacheProviderService"><see cref="ICacheProviderService"/>.</param>
        /// <param name="nomisHoldersService"><see cref="INomisHoldersService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public EtherlinkExplorerService(
            IOptions<BlacklistSettings> blacklistSettings,
            IOptions<EtherlinkExplorerSettings> settings,
            IEtherlinkExplorerClient client,
            IEtherlinkExplorerTestnetClient testnetClient,
            IScoringService scoringService,
            IReferralService referralService,
            IEvmScoreSoulboundTokenService soulboundTokenService,
            IDexProviderService dexProviderService,
            IDefiLlamaService defiLlamaService,
            IIPFSService ipfsService,
            IPolygonIdService polygonIdService,
            ICacheProviderService cacheProviderService,
            INomisHoldersService nomisHoldersService,
            ILogger<EtherlinkExplorerService> logger)
#pragma warning disable S3358
            : base(settings.Value.BlockchainDescriptors.TryGetValue(BlockchainKind.Mainnet, out var value) ? value : settings.Value.BlockchainDescriptors.TryGetValue(BlockchainKind.Testnet, out var testnetValue) ? testnetValue : null)
#pragma warning restore S3358
        {
            _settings = settings.Value;
            _blacklistSettings = blacklistSettings.Value;
            _client = client;
            _testnetClient = testnetClient;
            _scoringService = scoringService;
            _referralService = referralService;
            _soulboundTokenService = soulboundTokenService;
            _dexProviderService = dexProviderService;
            _defiLlamaService = defiLlamaService;
            _ipfsService = ipfsService;
            _polygonIdService = polygonIdService;
            _cacheProviderService = cacheProviderService;
            _nomisHoldersService = nomisHoldersService;
            Logger = logger;
        }

        /// <inheritdoc/>
        public ILogger Logger { get; }

        /// <inheritdoc/>
        public async Task<Result<TWalletScore>> GetWalletStatsAsync<TWalletStatsRequest, TWalletScore, TWalletStats, TTransactionIntervalData>(
            TWalletStatsRequest request,
            CancellationToken cancellationToken = default)
            where TWalletStatsRequest : WalletStatsRequest
            where TWalletScore : IWalletScore<TWalletStats, TTransactionIntervalData>, new()
            where TWalletStats : class, IWalletCommonStats<TTransactionIntervalData>, new()
            where TTransactionIntervalData : class, ITransactionIntervalData
        {
            if (!new AddressUtil().IsValidAddressLength(request.Address) || !new AddressUtil().IsValidEthereumAddressHexFormat(request.Address))
            {
                throw new InvalidAddressException(request.Address);
            }

            #region Blacklist

            if (_blacklistSettings.UseBlacklist)
            {
                var blacklist = new List<string>();
                foreach (var blacklistItem in _blacklistSettings.Blacklist)
                {
                    blacklist.AddRange(blacklistItem.Value);
                }

                if (blacklist.Contains(request.Address.ToLower()))
                {
                    throw new CustomException("The specified wallet address cannot be scored.", statusCode: HttpStatusCode.BadRequest);
                }
            }

            #endregion Blacklist

            var messages = new List<string>();

            #region Referral

            var ownReferralCodeResult = await _referralService.GetOwnReferralCodeAsync(request, _cacheProviderService, Logger, (request as BaseEvmWalletStatsRequest)?.ShouldGetReferrerCode ?? true, cancellationToken).ConfigureAwait(false);
            messages.AddRange(ownReferralCodeResult.Messages);
            string? ownReferralCode = ownReferralCodeResult.Data;

            #endregion Referral

            var mintBlockchain = _dexProviderService.MintChain(request, ChainId);

            TWalletStats? walletStats = null;
            bool calculateNewScore = false;
            if (_settings.GetFromCacheStatsIsEnabled && !request.DisableCache)
            {
                walletStats = await _cacheProviderService.GetFromCacheAsync<EtherlinkWalletStats>(request.GetWalletStatsCacheKey(ChainId)).ConfigureAwait(false) as TWalletStats;
            }

            var client = request.ScoringChainType == ScoringChainType.Mainnet ? _client : _testnetClient;
            if (walletStats == null)
            {
                calculateNewScore = true;
                string? balanceWei = (await client.GetBalanceAsync(request.Address).ConfigureAwait(false)).Balance;
                TokenPriceData? priceData = null;
                (await _defiLlamaService.TokensPriceAsync(new List<string> { $"coingecko:{PlatformIds?[BlockchainPlatform.Coingecko]}" }).ConfigureAwait(false))?.TokensPrices.TryGetValue($"coingecko:{PlatformIds?[BlockchainPlatform.Coingecko]}", out priceData);
                decimal usdBalance = (priceData?.Price ?? 0M) * balanceWei?.ToXtz() ?? 0;
                var transactions = (await client.GetTransactionsAsync<BaseEvmNormalTransactions, BaseEvmNormalTransaction>(request.Address).ConfigureAwait(false)).ToList();
                if (!transactions.Any())
                {
                    return await Result<TWalletScore>.FailAsync(
                        new()
                        {
                            Address = request.Address,
                            Stats = new TWalletStats
                            {
                                NoData = true
                            },
                            Score = 0
                        }, new List<string> { "There is no transactions for this wallet." }).ConfigureAwait(false);
                }

                var erc20Tokens = (await client.GetTransactionsAsync<BaseEvmERC20TokenTransfers, BaseEvmERC20TokenTransfer>(request.Address).ConfigureAwait(false)).ToList();

                #region Tokens data

                var tokenDataBalances = new List<TokenDataBalance>();
                if ((request as IWalletTokensBalancesRequest)?.GetHoldTokensBalances == true)
                {
                    foreach (string? erc20TokenContractId in erc20Tokens.Select(x => x.ContractAddress).Distinct())
                    {
                        var tokenDataBalance = await client.GetTokenDataBalanceAsync(request.Address, erc20TokenContractId!, ChainId).ConfigureAwait(false);
                        if (tokenDataBalance != null)
                        {
                            tokenDataBalances.Add(tokenDataBalance);
                        }
                    }
                }

                #endregion Tokens data

                #region Tokens balances

                var stablecoinsPrices = _dexProviderService.StablecoinsPriceData(ChainId);
                var pricesData = tokenDataBalances.Select(x => x.ToTokenPriceData()).Union(stablecoinsPrices).ToList();
                if (request is IWalletTokensBalancesRequest balancesRequest)
                {
                    tokenDataBalances = await tokenDataBalances
                        .EnrichWithDefiLlamaAsync(_defiLlamaService, balancesRequest, PlatformIds![BlockchainPlatform.DefiLLama], pricesData).ConfigureAwait(false);
                    tokenDataBalances = await tokenDataBalances
                        .EnrichWithTokensListsAsync(_dexProviderService, balancesRequest, ChainId).ConfigureAwait(false);

                    tokenDataBalances = tokenDataBalances
                        .Where(b => b.TotalAmountPrice > 0)
                        .OrderByDescending(b => b.TotalAmountPrice)
                        .ThenByDescending(b => b.Balance)
                        .ThenBy(b => b.Id)
                        .ThenBy(b => b.Symbol)
                        .DistinctBy(b => $"{b.Symbol}_{b.Price}_{b.Amount}")
                        .ToList();
                }

                #endregion Tokens balances

                #region Median USD balance

                decimal medianUsdBalance = await _scoringService.MedianBalanceUsdAsync<EtherlinkWalletStats>(request.Address, ChainId, request.CalculationModel, _settings, usdBalance + (tokenDataBalances.Any() ? tokenDataBalances : null)?.Sum(b => b.TotalAmountPrice) ?? 0, cancellationToken).ConfigureAwait(false);

                #endregion Median USD balance

                #region Tokens transfers balances

                var transferTokenDataBalances = new List<TransferTokenDataBalance>();
                if (request is IWalletTokensBalancesRequest { GetTokensTransfersBalances: true } tokensTransfersRequest)
                {
                    transferTokenDataBalances = erc20Tokens.ToTransferTokenDataBalances(transactions, request, ChainId);
                    pricesData = transferTokenDataBalances.Select(x => x.ToTokenPriceData()).Union(stablecoinsPrices).ToList();
                    transferTokenDataBalances = await transferTokenDataBalances
                        .EnrichWithDefiLlamaAsync(_defiLlamaService, tokensTransfersRequest, PlatformIds![BlockchainPlatform.DefiLLama], pricesData).ConfigureAwait(false);
                    transferTokenDataBalances = transferTokenDataBalances
                        .EnrichWithTokensBalances(tokenDataBalances);
                    transferTokenDataBalances = await transferTokenDataBalances
                        .EnrichWithTokensListsAsync(_dexProviderService, tokensTransfersRequest, ChainId).ConfigureAwait(false);

                    transferTokenDataBalances = transferTokenDataBalances
                        .Where(b => tokensTransfersRequest.ShowTokensTransfersWithZeroPrice || b.TotalAmountPrice > 0)
                        .OrderByDescending(b => b.TotalAmountPrice)
                        .ThenByDescending(b => b.Balance)
                        .ThenBy(b => b.Id)
                        .ThenBy(b => b.Symbol)
                        .DistinctBy(b => b.TransactionHash)
                        .ToList();
                }

                #endregion Tokens transfers balances

                walletStats = new EtherlinkStatCalculator(
                        request.Address,
                        decimal.TryParse(balanceWei, out decimal wei) ? wei : 0,
                        usdBalance,
                        medianUsdBalance,
                        transactions,
                        erc20Tokens,
                        tokenDataBalances,
                        transferTokenDataBalances)
                    .Stats() as TWalletStats;

                if (!request.DisableCache)
                {
                    await _cacheProviderService.SetCacheAsync(request.GetWalletStatsCacheKey(ChainId), walletStats!, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = _settings.GetFromCacheStatsTimeLimit
                    }).ConfigureAwait(false);
                }
            }

            double score = walletStats!.CalculateScore<TWalletStats, TTransactionIntervalData>(ChainId, request.CalculationModel);

            if (calculateNewScore && request is BaseEvmWalletStatsRequest { StoreScoreResults: true })
            {
                var scoringData = new ScoringData(request.Address, request.Address, request.CalculationModel, JsonSerializer.Serialize(request), ChainId, score, JsonSerializer.Serialize(walletStats));
                await _scoringService.SaveScoringDataToDatabaseAsync(scoringData, cancellationToken).ConfigureAwait(false);
            }

            var metadataResult = await _soulboundTokenService
                .TokenMetadataAsync(
                    _ipfsService,
                    _cacheProviderService,
                    request,
                    ChainId,
                    ChainName,
                    score,
                    new List<NFTTrait>(),
                    _settings.ImageVersion,
                    ScoreImageSkin.Default,
                    DateTime.UtcNow.ConvertToTimestamp(),
                    new List<NameService> { NameService.ETH })
                .ConfigureAwait(false);

            // getting discounted mint fee
            ulong? discountedMintFee = null;
            if (_settings.DiscountedMintFeeIsEnabled)
            {
                foreach (NomisHoldersScore scoreType in Enum.GetValuesAsUnderlyingType<NomisHoldersScore>())
                {
                    if (scoreType != NomisHoldersScore.None && _settings.DiscountedScores.TryGetValue(scoreType, out ulong? discountedScoreMintFee))
                    {
                        var holderData = await _nomisHoldersService.HolderAsync(scoreType, request.Address, cancellationToken: cancellationToken).ConfigureAwait(false);
                        if (holderData.IsHolder == true)
                        {
                            discountedMintFee = discountedScoreMintFee;
                            break;
                        }
                    }
                }
            }

            // getting signature
            ushort mintedScore = (ushort)(score * 10000);
            var signatureResult = await _soulboundTokenService
                .SignatureAsync(
                    request,
                    mintedScore,
                    mintBlockchain?.ChainId ?? request.GetChainId(_settings),
                    mintBlockchain?.SBTData ?? request.GetSBTData(_settings),
                    metadataResult.Data,
                    ChainId,
                    ownReferralCode ?? "anon",
                    request.ReferrerCode ?? "nomis",
                    discountedMintFee)
                .ConfigureAwait(false);

            messages.AddRange(signatureResult.Messages);
            messages.Add($"Got {ChainName} wallet {request.ScoreType.ToString()} score.");

            #region DID

            var didDataResult = await _polygonIdService.CreateClaimAndGetQrAsync<EtherlinkWalletStatsRequest, EtherlinkWalletStats, EtherlinkTransactionIntervalData>((request as EtherlinkWalletStatsRequest) !, mintedScore, (walletStats as EtherlinkWalletStats) !, DateTime.UtcNow.AddYears(5).ConvertToTimestamp(), ChainId, cancellationToken).ConfigureAwait(false);
            messages.Add(didDataResult.Messages.FirstOrDefault() ?? string.Empty);

            #endregion DID

            var scoreData = new TWalletScore
            {
                Address = request.Address,
                Stats = walletStats,
                Score = score,
                MintData = request.PrepareToMint ? new MintData(signatureResult.Data.Signature, mintedScore, request.CalculationModel, request.Deadline, metadataResult.Data, ChainId, mintBlockchain ?? this, ownReferralCode ?? "anon", request.ReferrerCode ?? "nomis") : null,
                DIDData = didDataResult.Data,
                ReferralCode = ownReferralCode,
                ReferrerCode = request.ReferrerCode
            };

            if (scoreData is IDiscountedWalletScore discountedWalletScore)
            {
                discountedWalletScore.DiscountedMintFee = discountedMintFee;
            }

            return await Result<TWalletScore>.SuccessAsync(scoreData, messages).ConfigureAwait(false);
        }
    }
}