// ------------------------------------------------------------------------------------------------------
// <copyright file="ScoringChainType.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

namespace Nomis.Utils.Enums
{
    /// <summary>
    /// Scoring chain type.
    /// </summary>
    public enum ScoringChainType :
        byte
    {
        /// <summary>
        /// Mainnet.
        /// </summary>
        Mainnet = 0,

        /// <summary>
        /// Testnet.
        /// </summary>
        Testnet
    }
}