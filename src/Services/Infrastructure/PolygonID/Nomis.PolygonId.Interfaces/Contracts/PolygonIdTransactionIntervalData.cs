// ------------------------------------------------------------------------------------------------------
// <copyright file="PolygonIdTransactionIntervalData.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json.Serialization;

using Nomis.Utils.Contracts;

namespace Nomis.PolygonId.Interfaces.Contracts
{
    /// <summary>
    /// PolygonId transaction interval data.
    /// </summary>
    /// <remarks>
    /// Plug.
    /// </remarks>
    public class PolygonIdTransactionIntervalData :
        ITransactionIntervalData
    {
        /// <inheritdoc />
        public decimal TokenUSDPrice { get; set; }

        /// <inheritdoc />
        public DateTime StartDate { get; set; }

        /// <inheritdoc />
        public DateTime EndDate { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public BigInteger AmountSum { get; set; }

        /// <inheritdoc cref="ITransactionIntervalData.AmountSumValue"/>
        public decimal AmountSumValue { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public decimal AmountSumUSDValue => AmountSumValue * TokenUSDPrice;

        /// <inheritdoc />
        [JsonIgnore]
        public BigInteger AmountOutSum { get; set; }

        /// <inheritdoc cref="ITransactionIntervalData.AmountOutSumValue"/>
        public decimal AmountOutSumValue { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public decimal AmountOutSumUSDValue => AmountOutSumValue * TokenUSDPrice;

        /// <inheritdoc />
        [JsonIgnore]
        public BigInteger AmountInSum { get; set; }

        /// <inheritdoc cref="ITransactionIntervalData.AmountInSumValue"/>
        public decimal AmountInSumValue { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public decimal AmountInSumUSDValue => AmountInSumValue * TokenUSDPrice;

        /// <inheritdoc />
        public int Count { get; set; }
    }
}