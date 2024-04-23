// ------------------------------------------------------------------------------------------------------
// <copyright file="NomisHoldersSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net;

using Nomis.Blockchain.Abstractions.Contracts.Settings;
using Nomis.NomisHolders.Interfaces.Enums;
using Nomis.Utils.Contracts.Common;
using Nomis.Utils.Contracts.Settings;

namespace Nomis.NomisHolders.Settings
{
    /// <summary>
    /// Nomis holders settings.
    /// </summary>
    internal class NomisHoldersSettings :
        ISettings,
        IRateLimitSettings,
        IHttpClientRetryingSettings
    {
        /// <summary>
        /// API base URL.
        /// </summary>
        public string? ApiBaseUrl { get; init; }

        /// <summary>
        /// Use subgraphs.
        /// </summary>
        public bool UseSubgraphs { get; set; }

        /// <summary>
        /// Subgraphs APIs.
        /// </summary>
        public IDictionary<NomisHoldersScore, string> SubgraphsApis { get; init; } = new Dictionary<NomisHoldersScore, string>();

        /// <inheritdoc/>
        public uint MaxApiCallsPerSecond { get; init; }

        #region IHttpClientRetryingSettings

        /// <inheritdoc/>
        public bool UseHttpClientRetrying { get; init; }

        /// <inheritdoc/>
        public int MaxRetries { get; init; }

        /// <inheritdoc/>
        public bool UseDefaultRetryTimeout { get; init; }

        /// <inheritdoc/>
        public TimeSpan DefaultRetryTimeout { get; init; }

        /// <inheritdoc/>
        public IDictionary<HttpStatusCode, TimeSpan> RetryTimeouts { get; init; } = new Dictionary<HttpStatusCode, TimeSpan>();

        #endregion #region IHttpClientRetryingSettings
    }
}