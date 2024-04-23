// ------------------------------------------------------------------------------------------------------
// <copyright file="NomisHolderDataRequest.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.NomisHolders.Interfaces.Requests
{
    /// <summary>
    /// Nomis holder data request.
    /// </summary>
    public class NomisHolderDataRequest
    {
        /// <summary>
        /// Holder wallet address.
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; } = null!;
    }
}