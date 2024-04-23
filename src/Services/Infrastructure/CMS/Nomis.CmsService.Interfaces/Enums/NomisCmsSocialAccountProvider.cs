// ------------------------------------------------------------------------------------------------------
// <copyright file="NomisCmsSocialAccountProvider.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace Nomis.CmsService.Interfaces.Enums
{
    /// <summary>
    /// Nomis CMS social account provider.
    /// </summary>
    public enum NomisCmsSocialAccountProvider :
        byte
    {
        /// <summary>
        /// Discord.
        /// </summary>
        [Description("discord")]
        Discord,

        /// <summary>
        /// Telegram.
        /// </summary>
        [Description("telegram")]
        Telegram,

        /// <summary>
        /// Twitter.
        /// </summary>
        [Description("twitter")]
        Twitter
    }
}