// ------------------------------------------------------------------------------------------------------
// <copyright file="IHasDiscountedMintFeeSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.NomisHolders.Interfaces.Enums;

namespace Nomis.Blockchain.Abstractions.Contracts.Settings
{
    /// <summary>
    /// Discounted mint fee settings.
    /// </summary>
    public interface IHasDiscountedMintFeeSettings
    {
        /// <summary>
        /// Discounted mint fee is enabled.
        /// </summary>
        public bool DiscountedMintFeeIsEnabled { get; init; }

        /// <summary>
        /// Discounted scores with its discounted mint fees.
        /// </summary>
        public IDictionary<NomisHoldersScore, ulong?> DiscountedScores { get; init; }
    }
}