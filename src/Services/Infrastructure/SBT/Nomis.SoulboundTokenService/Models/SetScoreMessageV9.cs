// ------------------------------------------------------------------------------------------------------
// <copyright file="SetScoreMessageV9.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Nomis.SoulboundTokenService.Models
{
    /// <summary>
    /// Set score message.
    /// </summary>
    [Struct(nameof(SetScoreMessage))]
    public class SetScoreMessageV9 :
        SetScoreMessageV8
    {
        /// <summary>
        /// Discounted mint fee.
        /// </summary>
        [Parameter("uint256", "discountedMintFee", 10)]
        public BigInteger DiscountedMintFee { get; set; }
    }
}