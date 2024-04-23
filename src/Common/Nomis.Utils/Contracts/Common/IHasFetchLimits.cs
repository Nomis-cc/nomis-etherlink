// ------------------------------------------------------------------------------------------------------
// <copyright file="IHasFetchLimits.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

namespace Nomis.Utils.Contracts.Common
{
    /// <summary>
    /// Has fetch limits.
    /// </summary>
    public interface IHasFetchLimits
    {
        /// <summary>
        /// Items fetch limit per request.
        /// </summary>
        public int? ItemsFetchLimitPerRequest { get; init; }

        /// <summary>
        /// Counted transaction limit.
        /// </summary>
        public int? TransactionsLimit { get; init; }
    }
}