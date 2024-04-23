// ------------------------------------------------------------------------------------------------------
// <copyright file="ScoringBaseSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Contracts.Settings;
using Nomis.Utils.Contracts.Common;
using Nomis.Utils.Contracts.Proxy;
using Nomis.Utils.Contracts.Settings;

namespace Nomis.Blockchain.Abstractions.Settings
{
    /// <summary>
    /// Scoring base settings.
    /// </summary>
    public class ScoringBaseSettings :
        IHasHttpClientProxiesSettings,
        IRateLimitSettings,
        IHasFetchLimits
    {
        /// <summary>
        /// Is testnet.
        /// </summary>
        public bool IsTestnet { get; init; }

        /// <summary>
        /// API base URL.
        /// </summary>
        public string? ApiBaseUrl { get; init; }

        /// <summary>
        /// Appended path for API base URL.
        /// </summary>
        public string? AppendedPath { get; init; }

        /// <summary>
        /// API keys.
        /// </summary>
        public IList<string> ApiKeys { get; init; } = new List<string>();

        /// <inheritdoc />
        public bool UseProxies { get; init; } = true;

        /// <inheritdoc />
        public IList<HttpClientProxy> HttpClientProxies { get; init; } = new List<HttpClientProxy>();

        /// <inheritdoc/>
        public uint MaxApiCallsPerSecond { get; init; }

        /// <inheritdoc/>
        public int? ItemsFetchLimitPerRequest { get; init; }

        /// <inheritdoc/>
        public int? TransactionsLimit { get; init; }
    }
}