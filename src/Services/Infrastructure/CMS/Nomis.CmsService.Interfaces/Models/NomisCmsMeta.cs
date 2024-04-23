// ------------------------------------------------------------------------------------------------------
// <copyright file="NomisCmsMeta.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.CmsService.Interfaces.Models
{
    /// <summary>
    /// Nomis CMS metadata.
    /// </summary>
    public class NomisCmsMeta
    {
        /// <summary>
        /// Nomis CMS pagination data.
        /// </summary>
        [JsonPropertyName("pagination")]
        public NomisCmsMetaPagination? Pagination { get; set; }
    }
}