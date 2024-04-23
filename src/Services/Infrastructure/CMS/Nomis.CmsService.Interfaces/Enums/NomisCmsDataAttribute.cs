// ------------------------------------------------------------------------------------------------------
// <copyright file="NomisCmsDataAttribute.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace Nomis.CmsService.Interfaces.Enums
{
    /// <summary>
    /// Nomis CMS data attribute.
    /// </summary>
    public enum NomisCmsDataAttribute
    {
        /// <summary>
        /// EVM wallet address.
        /// </summary>
        [Description("wallet")]
        Wallet,

        /// <summary>
        /// Solana wallet address.
        /// </summary>
        [Description("solanaWallet")]
        SolanaWallet,

        /// <summary>
        /// Referrer code.
        /// </summary>
        [Description("referrerCode")]
        ReferrerCode,

        /// <summary>
        /// Referral code.
        /// </summary>
        [Description("referralCode")]
        ReferralCode,

        /// <summary>
        /// Referral count.
        /// </summary>
        [Description("referralCount")]
        ReferralCount,

        /// <summary>
        /// Points.
        /// </summary>
        [Description("points")]
        Points,

        /// <summary>
        /// Score.
        /// </summary>
        [Description("score")]
        Score,

        /// <summary>
        /// Rank.
        /// </summary>
        [Description("rank")]
        Rank
    }
}