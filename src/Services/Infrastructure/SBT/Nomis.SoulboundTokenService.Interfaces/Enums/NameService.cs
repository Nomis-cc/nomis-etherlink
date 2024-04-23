// ------------------------------------------------------------------------------------------------------
// <copyright file="NameService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace Nomis.SoulboundTokenService.Interfaces.Enums
{
    /// <summary>
    /// Name service.
    /// </summary>
    public enum NameService
    {
        /// <summary>
        /// ETH.
        /// </summary>
        [Description("eth")]
        ETH,

        /// <summary>
        /// BNB.
        /// </summary>
        [Description("bnb")]
        BNB,

        /// <summary>
        /// ARB.
        /// </summary>
        [Description("arb")]
        ARB
    }
}