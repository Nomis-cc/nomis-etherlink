// ------------------------------------------------------------------------------------------------------
// <copyright file="IDiscountedWalletScore.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Blockchain.Abstractions
{
    /// <summary>
    /// Discounted wallet score.
    /// </summary>
    public interface IDiscountedWalletScore
    {
        /// <summary>
        /// Discounted mint fee.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ulong? DiscountedMintFee { get; set; }
    }
}