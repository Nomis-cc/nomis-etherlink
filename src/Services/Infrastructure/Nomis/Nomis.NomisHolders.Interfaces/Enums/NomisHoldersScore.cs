// ------------------------------------------------------------------------------------------------------
// <copyright file="NomisHoldersScore.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace Nomis.NomisHolders.Interfaces.Enums
{
    /// <summary>
    /// Nomis holders score.
    /// </summary>
    public enum NomisHoldersScore
    {
        /// <summary>
        /// None.
        /// </summary>
        None,

        /// <summary>
        /// Multichain score.
        /// </summary>
        [Description("multichain")]
        Multichain,

        /// <summary>
        /// zkSync Era score.
        /// </summary>
        [Description("zksync")]
        ZkSync,

        /// <summary>
        /// LayerZero score.
        /// </summary>
        [Description("layerzero")]
        LayerZero,

        /// <summary>
        /// Rubic score.
        /// </summary>
        [Description("rubic")]
        Rubic,

        /// <summary>
        /// Polygon zkEVM score.
        /// </summary>
        [Description("polygon-zkevm")]
        ZkEvm,

        /// <summary>
        /// Linea score.
        /// </summary>
        [Description("linea")]
        Linea,

        /// <summary>
        /// Strateg score.
        /// </summary>
        [Description("strateg")]
        Strateg,

        /// <summary>
        /// Mantle score.
        /// </summary>
        [Description("mantle")]
        Mantle,

        /// <summary>
        /// opBNB score.
        /// </summary>
        [Description("opbnb")]
        OpBnb,

        /// <summary>
        /// Scroll score.
        /// </summary>
        [Description("scroll")]
        Scroll,

        /// <summary>
        /// Manta score.
        /// </summary>
        [Description("manta")]
        Manta,

        /// <summary>
        /// ZKFair score.
        /// </summary>
        [Description("zkfair")]
        ZkFair,

        /// <summary>
        /// Blast Sepolia score.
        /// </summary>
        [Description("blast-sepolia")]
        BlastSepolia,

        /// <summary>
        /// Mode score.
        /// </summary>
        [Description("mode")]
        Mode,

        /// <summary>
        /// Berachain Artio score.
        /// </summary>
        [Description("berachain-artio")]
        BerachainArtio,

        /// <summary>
        /// Starknet score.
        /// </summary>
        [Description("starknet")]
        Starknet,

        /// <summary>
        /// Blast score.
        /// </summary>
        [Description("blast")]
        Blast,

        /// <summary>
        /// Kroma score.
        /// </summary>
        [Description("kroma")]
        Kroma,

        /// <summary>
        /// Eywa score.
        /// </summary>
        [Description("eywa")]
        Eywa,

        /// <summary>
        /// Plume Testnet score.
        /// </summary>
        [Description("plume-testnet")]
        PlumeTestnet,

        /// <summary>
        /// Plume score.
        /// </summary>
        [Description("plume")]
        Plume,

        /// <summary>
        /// UniLayer Testnet score.
        /// </summary>
        [Description("unilayer-testnet")]
        UniLayerTestnet,

        /// <summary>
        /// UniLayer score.
        /// </summary>
        [Description("unilayer")]
        UniLayer,

        /// <summary>
        /// Botanix Testnet score.
        /// </summary>
        [Description("botanix-testnet")]
        BotanixTestnet,

        /// <summary>s
        /// Botanix score.
        /// </summary>
        [Description("botanix")]
        Botanix,

        /// <summary>
        /// Lisk Sepolia Testnet score.
        /// </summary>
        [Description("lisk-sepolia")]
        LiskTestnet,

        /// <summary>
        /// Lisk score.
        /// </summary>
        [Description("lisk")]
        Lisk,

        /// <summary>
        /// ZkLink Nova score.
        /// </summary>
        [Description("zklink-nova")]
        ZkLinkNova
    }
}