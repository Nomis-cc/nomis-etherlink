// ------------------------------------------------------------------------------------------------------
// <copyright file="NomisHoldersData.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.NomisHolders.Interfaces.Models
{
    /// <summary>
    /// Nomis holders data.
    /// </summary>
    public class NomisHoldersData
    {
        /// <summary>
        /// Message.
        /// </summary>
        [JsonPropertyName("message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }

        /// <summary>
        /// Owner wallet address.
        /// </summary>
        [JsonPropertyName("owner")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Owner { get; set; }

        /// <summary>
        /// Is holder.
        /// </summary>
        [JsonPropertyName("isHolder")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsHolder { get; set; }

        /// <summary>
        /// Blockchain id.
        /// </summary>
        [JsonPropertyName("chainId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ulong? ChainId { get; set; }

        /// <summary>
        /// Smart-contract version.
        /// </summary>
        [JsonPropertyName("version")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Version { get; set; }

        /// <summary>
        /// Calculation model.
        /// </summary>
        [JsonPropertyName("calculationModel")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? CalculationModel { get; set; }

        /// <summary>
        /// Score.
        /// </summary>
        [JsonPropertyName("score")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Score { get; set; }

        /// <summary>
        /// Token id.
        /// </summary>
        [JsonPropertyName("tokenId")]
        public int? TokenId { get; set; }

        /// <summary>
        /// Updated timestamp.
        /// </summary>
        [JsonPropertyName("updated_s")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Updated { get; set; }

        /// <summary>
        /// Updated timestamp.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? UpdatedDateTime =>
            Updated.HasValue ? DateTimeOffset.FromUnixTimeSeconds(Updated.Value).DateTime : null;
    }
}