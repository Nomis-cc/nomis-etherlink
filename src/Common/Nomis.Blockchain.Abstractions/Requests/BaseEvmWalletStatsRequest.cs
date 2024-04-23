﻿// ------------------------------------------------------------------------------------------------------
// <copyright file="BaseEvmWalletStatsRequest.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nomis.PolygonId.Interfaces.Contracts;
using Nomis.Utils.Contracts.Requests;

// ReSharper disable VirtualMemberCallInConstructor
namespace Nomis.Blockchain.Abstractions.Requests
{
    /// <summary>
    /// Base EVM wallet stats request.
    /// </summary>
    public abstract class BaseEvmWalletStatsRequest :
        WalletStatsRequest,
        IWalletTokensBalancesRequest,
        IWalletPolygonIdRequest,
        IWalletCounterpartiesRequest,
        IDiscordSendingRequest
    {
        /// <inheritdoc />
        /// <example>false</example>
        [FromQuery]
        [JsonPropertyOrder(-15)]
        public virtual bool UseAllCounterparties { get; set; } = false;

        /// <inheritdoc />
        /// <example>false</example>
        [FromQuery]
        [JsonPropertyOrder(-14)]
        public virtual bool CalculateOnlyCounterparties { get; set; } = false;

        /// <inheritdoc />
        /// <example>true</example>
        [FromQuery]
        [JsonPropertyOrder(-13)]
        public virtual bool GetHoldTokensBalances { get; set; } = true;

        /// <inheritdoc />
        /// <example>6</example>
        [FromQuery]
        [Range(typeof(int), "1", "8760")]
        [JsonPropertyOrder(-12)]
        public virtual int SearchWidthInHours { get; set; } = 6;

        /// <inheritdoc />
        /// <example>false</example>
        [FromQuery]
        [JsonPropertyOrder(-11)]
        public virtual bool UseTokenLists { get; set; } = false;

        /// <inheritdoc />
        /// <example>false</example>
        [FromQuery]
        [JsonPropertyOrder(-10)]
        public virtual bool IncludeUniversalTokenLists { get; set; } = false;

        /// <inheritdoc />
        /// <example>false</example>
        [FromQuery]
        [JsonPropertyOrder(-4)]
        public virtual bool UseDIDStorage { get; set; }

        /// <inheritdoc />
        /// <example>did:polygonid:polygon:mumbai:2qEPoWiBpDYkjKscVBNF8KZLg66gs7vjkYAZSNBHX2</example>
        [FromQuery]
        [JsonPropertyOrder(-3)]
        public virtual string? DID { get; set; }

        /// <summary>
        /// Use Covalent API for getting token holding.
        /// </summary>
        /// <example>true</example>
        [FromQuery]
        public virtual bool UseCovalentApi { get; init; } = true;

        /// <summary>
        /// Use DeBank API for getting token holding.
        /// </summary>
        /// <example>true</example>
        [FromQuery]
        public virtual bool UseDeBankApi { get; init; } = true;

        /// <summary>
        /// Use DeBank API for getting tokens prices.
        /// </summary>
        /// <example>false</example>
        [FromQuery]
        public virtual bool UseDeBankPriceApi { get; init; } = false;

        /// <summary>
        /// Use De.Fi API for getting token holding.
        /// </summary>
        /// <example>false</example>
        [FromQuery]
        public virtual bool UseDeFiApi { get; init; } = false;

        /// <summary>
        /// Store score results to DB.
        /// </summary>
        /// <example>true</example>
        [JsonIgnore]
        public virtual bool StoreScoreResults { get; set; } = true;

        /// <summary>
        /// Should get referrer code.
        /// </summary>
        /// <example>true</example>
        [JsonIgnore]
        public virtual bool ShouldGetReferrerCode { get; set; } = true;

        /// <summary>
        /// Disable RPC balance checker.
        /// </summary>
        /// <example>false</example>
        [JsonIgnore]
        public virtual bool DisableRpcBalanceChecker { get; set; } = false;

        /// <summary>
        /// Use SocketScan API.
        /// </summary>
        /// <example>false</example>
        [BindNever]
        public virtual bool UseSocketScanApi { get; init; } = false;

        /// <inheritdoc />
        [BindNever]
        public virtual bool SendScoreToDiscord { get; set; } = true;

        /// <inheritdoc />
        /// <example>false</example>
        [FromQuery]
        public virtual bool GetTokensTransfersBalances { get; set; } = false;

        /// <inheritdoc />
        /// <example>false</example>
        [FromQuery]
        public virtual bool ShowTokensTransfersWithZeroPrice { get; set; } = false;

        /// <inheritdoc />
        /// <example>6</example>
        [FromQuery]
        [Range(typeof(int), "1", "8760")]
        public virtual int TransfersSearchWidthInHours { get; set; } = 6;
    }
}