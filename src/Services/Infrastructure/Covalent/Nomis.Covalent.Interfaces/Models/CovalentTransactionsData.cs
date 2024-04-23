// ------------------------------------------------------------------------------------------------------
// <copyright file="CovalentTransactionsData.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Covalent.Interfaces.Models
{
    /// <summary>
    /// Covalent transactions data.
    /// </summary>
    public class CovalentTransactionsData
    {
        /// <summary>
        /// Address.
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; } = null!;

        /// <summary>
        /// Updated at.
        /// </summary>
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Transactions data list.
        /// </summary>
        [JsonPropertyName("items")]
        public IList<CovalentTransactionData> Transactions { get; set; } = new List<CovalentTransactionData>();

        /// <summary>
        /// Navigation links.
        /// </summary>
        [JsonPropertyName("links")]
        public CovalentNavigationLinks? Links { get; set; }
    }
}