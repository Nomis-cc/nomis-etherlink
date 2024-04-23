// ------------------------------------------------------------------------------------------------------
// <copyright file="PolygonIdWalletFakeStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

using Nomis.Utils.Contracts;
using Nomis.Utils.Contracts.Stats;
using Nomis.Utils.Enums;

namespace Nomis.PolygonId.Interfaces.Contracts
{
    /// <summary>
    /// PolygonId wallet fake stats.
    /// </summary>
    /// <remarks>
    /// Plug.
    /// </remarks>
    public class PolygonIdWalletFakeStats :
        IWalletCommonStats<PolygonIdTransactionIntervalData>
    {
        /// <inheritdoc />
        [JsonIgnore]
        public bool NoData { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public IEnumerable<Func<ulong, ScoringCalculationModel, double>> AdditionalScores => new List<Func<ulong, ScoringCalculationModel, double>>();

        /// <inheritdoc />
        [JsonIgnore]
        public IEnumerable<Func<ulong, ScoringCalculationModel, double>> AdjustingScoreMultipliers => new List<Func<ulong, ScoringCalculationModel, double>>();

        /// <inheritdoc />
        [JsonIgnore]
        public int WalletAge { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public DateTime ScoredAt { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public IEnumerable<PolygonIdTransactionIntervalData>? TurnoverIntervals { get; set; } = new List<PolygonIdTransactionIntervalData>();

        /// <inheritdoc />
        [JsonIgnore]
        public IDictionary<string, PropertyData> StatsDescriptions { get; } = new Dictionary<string, PropertyData>();
    }
}