// ------------------------------------------------------------------------------------------------------
// <copyright file="NomisSocialAccountProvider.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace Nomis.NomisHolders.Interfaces.Enums
{
    /// <summary>
    /// Social account provider.
    /// </summary>
    public enum NomisSocialAccountProvider :
        byte
    {
        /// <summary>
        /// Discord.
        /// </summary>
        [Description("discord")]
        Discord,

        /// <summary>
        /// Twitter (X).
        /// </summary>
        [Description("twitter")]
        Twitter
    }
}