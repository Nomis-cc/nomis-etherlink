// ------------------------------------------------------------------------------------------------------
// <copyright file="EtherlinkHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Numerics;

namespace Nomis.EtherlinkExplorer.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for Etherlink.
    /// </summary>
    public static class EtherlinkHelpers
    {
        /// <summary>
        /// Convert Wei value to XTZ.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total XTZ.</returns>
        public static decimal ToNative(this in BigInteger valueInWei)
        {
            if (valueInWei > new BigInteger(decimal.MaxValue))
            {
                return (decimal)(valueInWei / new BigInteger(100_000_000_000_000_000));
            }

            return (decimal)valueInWei * 0.000_000_000_000_000_001M;
        }

        /// <summary>
        /// Convert native value to wei.
        /// </summary>
        /// <param name="value">Native value.</param>
        /// <returns>Returns total wei.</returns>
        public static BigInteger FromNative(this in decimal value)
        {
            if (value > decimal.MaxValue / 100_000_000_000_000_000)
            {
                return (BigInteger)(value * new decimal(100_000_000_000_000_000));
            }

            return (BigInteger)(value / 0.000_000_000_000_000_001M);
        }

        /// <summary>
        /// Convert Wei value to XTZ.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total XTZ.</returns>
        public static decimal ToXtz(this string valueInWei)
        {
            return BigInteger
                .TryParse(valueInWei, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { CurrencyDecimalSeparator = "." }, out var value)
                ? value.ToNative()
                : 0;
        }

        /// <summary>
        /// Convert Wei value to XTZ.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total XTZ.</returns>
        public static decimal ToXtz(this decimal valueInWei)
        {
            return new BigInteger(valueInWei).ToNative();
        }
    }
}