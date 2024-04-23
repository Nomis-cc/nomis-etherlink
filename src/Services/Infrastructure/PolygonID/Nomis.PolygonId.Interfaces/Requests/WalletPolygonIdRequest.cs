// ------------------------------------------------------------------------------------------------------
// <copyright file="WalletPolygonIdRequest.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nomis.PolygonId.Interfaces.Contracts;
using Nomis.Utils.Contracts.Requests;
using Nomis.Utils.Enums;

namespace Nomis.PolygonId.Interfaces.Requests
{
    /// <inheritdoc cref="IWalletPolygonIdRequest"/>
    public class WalletPolygonIdRequest :
        WalletStatsRequest,
        IWalletPolygonIdRequest
    {
        /// <inheritdoc />
        /// <example>true</example>
        [BindNever]
        public virtual bool UseDIDStorage { get; set; } = true;

        /// <inheritdoc />
        public virtual string? DID { get; set; }

        /// <inheritdoc />
        [BindNever]
        public override ulong Nonce { get; set; }

        /// <inheritdoc />
        [BindNever]
        public override ulong Deadline { get; set; }

        /// <inheritdoc />
        [BindNever]
        public override ScoringChainType ScoringChainType { get; set; } = ScoringChainType.Mainnet;

        /// <inheritdoc />
        [BindNever]
        public override ScoringCalculationModel CalculationModel { get; set; } = ScoringCalculationModel.CommonV3;

        /// <inheritdoc />
        [BindNever]
        public override ScoreType ScoreType { get; set; } = ScoreType.Finance;

        /// <inheritdoc />
        [BindNever]
        public override string? TokenAddress { get; set; }

        /// <inheritdoc />
        [BindNever]
        public override bool PrepareToMint { get; set; }

        /// <inheritdoc />
        [BindNever]
        public override string? ReferrerCode { get; set; }

        /// <inheritdoc />
        [BindNever]
        public override MintChain MintChain { get; set; } = MintChain.Native;

        /// <inheritdoc />
        [BindNever]
        public override MintChainType MintBlockchainType { get; set; } = MintChainType.Mainnet;

        /// <inheritdoc />
        [BindNever]
        public override bool DisableCache { get; set; } = false;

        /// <inheritdoc />
        [BindNever]
        public override bool DisableProxy { get; set; } = false;
    }
}