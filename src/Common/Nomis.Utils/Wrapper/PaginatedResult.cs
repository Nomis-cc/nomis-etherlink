﻿// ------------------------------------------------------------------------------------------------------
// <copyright file="PaginatedResult.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

using Nomis.Utils.Contracts.Common;

namespace Nomis.Utils.Wrapper
{
    /// <summary>
    /// Operation result with paginated data.
    /// </summary>
    /// <typeparam name="TData">Data type.</typeparam>
    public record PaginatedResult<TData> :
        Result,
        IPaginated
    {
        /// <summary>
        /// Initialize <see cref="PaginatedResult{TData}"/>.
        /// </summary>
        /// <param name="data">List of data.</param>
        public PaginatedResult(IList<TData> data)
        {
            Data = data;
        }

        /// <summary>
        /// Initialize <see cref="PaginatedResult{T}"/>.
        /// </summary>
        /// <param name="succeeded">Operation is successed.</param>
        /// <param name="data">List of data.</param>
        /// <param name="messages">Message list.</param>
        /// <param name="count">Total count of data.</param>
        /// <param name="page">Current page number.</param>
        /// <param name="pageSize">Count of data per page.</param>
        internal PaginatedResult(
            bool succeeded,
            IList<TData>? data = default,
            List<string>? messages = null,
            int count = 0,
            int page = 1,
            int pageSize = 10)
        {
            Data = data;
            Messages = messages ?? new List<string>();
            PageNumber = page;
            Succeeded = succeeded;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
        }

        /// <summary>
        /// List of data.
        /// </summary>
        [JsonInclude]
        public IList<TData>? Data { get; private set; }

        /// <inheritdoc cref="IPaginated.PageNumber"/>
        [JsonInclude]
        public int PageNumber { get; private set; }

        /// <summary>
        /// Total count of pages.
        /// </summary>
        [JsonInclude]
        public int TotalPages { get; private set; }

        /// <summary>
        /// Total count of data.
        /// </summary>
        [JsonInclude]
        public int TotalCount { get; private set; }

        /// <inheritdoc cref="IPaginated.PageSize"/>
        [JsonInclude]
        public int PageSize { get; private set; }

        /// <summary>
        /// Has previous page.
        /// </summary>
        [JsonInclude]
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Has next page.
        /// </summary>
        [JsonInclude]
        public bool HasNextPage => PageNumber < TotalPages;

        /// <summary>
        /// Fail operation result with paginated data.
        /// </summary>
        /// <param name="messages">Message (error) list.</param>
        /// <returns>Returns <see cref="PaginatedResult{T}"/>.</returns>
        public static PaginatedResult<TData> Failure(IList<string> messages)
        {
            return new(false, default, messages.ToList());
        }

        /// <summary>
        /// Success operation result with paginated data.
        /// </summary>
        /// <param name="data">List of data.</param>
        /// <param name="count">Total count of data.</param>
        /// <param name="page">Current page number.</param>
        /// <param name="pageSize">Count of data per page.</param>
        /// <returns>Returns <see cref="PaginatedResult{T}"/>.</returns>
        public static PaginatedResult<TData> Success(IList<TData> data, int count, int page, int pageSize)
        {
            return new(true, data, null, count, page, pageSize);
        }
    }
}