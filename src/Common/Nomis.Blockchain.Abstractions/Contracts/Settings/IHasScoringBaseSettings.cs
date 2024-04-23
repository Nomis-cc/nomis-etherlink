// ------------------------------------------------------------------------------------------------------
// <copyright file="IHasScoringBaseSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Settings;
using Nomis.Utils.Enums;

namespace Nomis.Blockchain.Abstractions.Contracts.Settings
{
    /// <summary>
    /// Has scoring base settings.
    /// </summary>
    public interface IHasScoringBaseSettings
    {
        /// <summary>
        /// Scoring settings for different scoring chain types.
        /// </summary>
        public IDictionary<ScoringChainType, ScoringBaseSettings> ScoringSettings { get; init; }
    }
}