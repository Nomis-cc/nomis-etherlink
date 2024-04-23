// ------------------------------------------------------------------------------------------------------
// <copyright file="NomisHolderSubgraphData.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.NomisHolders.Interfaces.Models
{
    /// <summary>
    /// Nomis holder subgraph data.
    /// </summary>
    public class NomisHolderSubgraphData
    {
        /// <summary>
        /// Token id.
        /// </summary>
        [JsonPropertyName("tokenId")]
        public string? TokenId { get; set; }

        /// <summary>
        /// Owner wallet address.
        /// </summary>
        [JsonPropertyName("owner")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Owner { get; set; }

        /// <summary>
        /// Score.
        /// </summary>
        [JsonPropertyName("score")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Score { get; set; }

        /// <summary>
        /// Calculation model.
        /// </summary>
        [JsonPropertyName("calculationModel")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? CalculationModel { get; set; }

        /// <summary>
        /// Blockchain id.
        /// </summary>
        [JsonPropertyName("chainId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ChainId { get; set; }

        /// <summary>
        /// Updated timestamp.
        /// </summary>
        [JsonPropertyName("blockTimestamp")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Updated { get; set; }
    }
}