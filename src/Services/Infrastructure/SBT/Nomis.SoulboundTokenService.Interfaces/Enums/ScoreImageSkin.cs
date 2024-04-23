// ------------------------------------------------------------------------------------------------------
// <copyright file="ScoreImageSkin.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace Nomis.SoulboundTokenService.Interfaces.Enums
{
    /// <summary>
    /// Score image skin.
    /// </summary>
    public enum ScoreImageSkin
    {
        /// <summary>
        /// Default skin.
        /// </summary>
        [Description("default")]
        Default = 0,

        /// <summary>
        /// Blast skin.
        /// </summary>
        [Description("blast")]
        Blast = 1,
    }
}