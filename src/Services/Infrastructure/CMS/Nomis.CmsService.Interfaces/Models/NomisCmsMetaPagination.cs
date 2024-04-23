// ------------------------------------------------------------------------------------------------------
// <copyright file="NomisCmsMetaPagination.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.CmsService.Interfaces.Models
{
    /// <summary>
    /// Nomis CMS pagination data.
    /// </summary>
    public class NomisCmsMetaPagination
    {
        /// <summary>
        /// Current page number.
        /// </summary>
        [JsonPropertyName("page")]
        public int Page { get; set; }

        /// <summary>
        /// Page size.
        /// </summary>
        /// <remarks>
        /// Elements per page.
        /// </remarks>
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// Total page count.
        /// </summary>
        [JsonPropertyName("pageCount")]
        public int PageCount { get; set; }

        /// <summary>
        /// Total data elements.
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}