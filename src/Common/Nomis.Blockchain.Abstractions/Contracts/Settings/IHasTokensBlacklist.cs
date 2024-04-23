// ------------------------------------------------------------------------------------------------------
// <copyright file="IHasTokensBlacklist.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

namespace Nomis.Blockchain.Abstractions.Contracts.Settings
{
    /// <summary>
    /// Hast tokens blacklist.
    /// </summary>
    public interface IHasTokensBlacklist
    {
        /// <summary>
        /// Blacklist token Ids.
        /// </summary>
        public IList<string> BlacklistTokenIds { get; init; }
    }
}