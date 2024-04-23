// ------------------------------------------------------------------------------------------------------
// <copyright file="CovalentNavigationLinks.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Covalent.Interfaces.Models
{
    /// <summary>
    /// Covalent navigation links.
    /// </summary>
    public class CovalentNavigationLinks
    {
        /// <summary>
        /// Previous link.
        /// </summary>
        [JsonPropertyName("prev")]
        public string? Prev { get; set; }

        /// <summary>
        /// Next link.
        /// </summary>
        [JsonPropertyName("next")]
        public string? Next { get; set; }
    }
}