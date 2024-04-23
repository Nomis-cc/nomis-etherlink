// ------------------------------------------------------------------------------------------------------
// <copyright file="CovalentTransactionData.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Covalent.Interfaces.Models
{
    /// <summary>
    /// Covalent transaction data.
    /// </summary>
    public class CovalentTransactionData
    {
        /// <summary>
        /// Block time.
        /// </summary>
        [JsonPropertyName("block_signed_at")]
        public DateTime BlockTime { get; set; }

        /// <summary>
        /// Block number.
        /// </summary>
        [JsonPropertyName("block_height")]
        public ulong BlockNumber { get; set; }

        /// <summary>
        /// Transaction hash.
        /// </summary>
        [JsonPropertyName("tx_hash")]
        public virtual string Hash { get; set; } = null!;

        /// <summary>
        /// From address.
        /// </summary>
        [JsonPropertyName("from_address")]
        public virtual string? From { get; set; }

        /// <summary>
        /// To address.
        /// </summary>
        [JsonPropertyName("to_address")]
        public virtual string? To { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        [JsonPropertyName("value")]
        public virtual string? Value { get; set; }

        /// <summary>
        /// Is successful.
        /// </summary>
        [JsonPropertyName("successful")]
        public virtual bool Successful { get; set; }
    }
}