// ------------------------------------------------------------------------------------------------------
// <copyright file="NomisCmsResponse.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

using Nomis.CmsService.Interfaces.Models;

namespace Nomis.CmsService.Interfaces.Responses
{
    /// <summary>
    /// Nomis CMS referral response.
    /// </summary>
    public class NomisCmsResponse
    {
        /// <summary>
        /// List of referral data.
        /// </summary>
        [JsonPropertyName("data")]
        public IEnumerable<NomisCmsData> Data { get; set; } = new List<NomisCmsData>();

        /// <summary>
        /// Response metadata.
        /// </summary>
        [JsonPropertyName("meta")]
        public NomisCmsMeta? Meta { get; set; }
    }
}