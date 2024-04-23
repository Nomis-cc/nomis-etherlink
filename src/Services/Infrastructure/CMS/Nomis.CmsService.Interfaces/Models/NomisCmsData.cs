// ------------------------------------------------------------------------------------------------------
// <copyright file="NomisCmsData.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.CmsService.Interfaces.Models
{
    /// <summary>
    /// Nomis CMS data.
    /// </summary>
    public class NomisCmsData
    {
        /// <summary>
        /// Entity id.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Attributes.
        /// </summary>
        [JsonPropertyName("attributes")]
        public IDictionary<string, object?> Attributes { get; set; } = new Dictionary<string, object?>();
    }
}