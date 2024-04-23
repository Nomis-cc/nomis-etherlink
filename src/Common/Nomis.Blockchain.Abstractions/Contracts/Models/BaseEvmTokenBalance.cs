// ------------------------------------------------------------------------------------------------------
// <copyright file="BaseEvmTokenBalance.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Blockchain.Abstractions.Contracts.Models
{
    /// <summary>
    /// Base EVM token balance.
    /// </summary>
    public class BaseEvmTokenBalance
    {
        /// <summary>
        /// Token data.
        /// </summary>
        [JsonPropertyName("token")]
        public BaseEvmToken? Token { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}