// ------------------------------------------------------------------------------------------------------
// <copyright file="IDiscordSendingRequest.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

namespace Nomis.Utils.Contracts.Requests
{
    /// <summary>
    /// Discord sending request.
    /// </summary>
    public interface IDiscordSendingRequest
    {
        /// <summary>
        /// Send score calculation results to Discord.
        /// </summary>
        public bool SendScoreToDiscord { get; set; }
    }
}