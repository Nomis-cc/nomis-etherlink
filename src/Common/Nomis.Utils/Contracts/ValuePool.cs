// ------------------------------------------------------------------------------------------------------
// <copyright file="ValuePool.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Collections.Concurrent;

namespace Nomis.Utils.Contracts
{
    /// <inheritdoc cref="IValuePool{TService, TValue}"/>
    public class ValuePool<TService, TValue> :
        IValuePool<TService, TValue>
        where TService : class
        where TValue : class
    {
        private readonly IDictionary<int, IList<TValue>> _values = new ConcurrentDictionary<int, IList<TValue>>();
        private readonly IDictionary<int, int> _currentValueIndex = new ConcurrentDictionary<int, int>();

        /// <summary>
        /// Initialize <see cref="ValuePool{TService, TValue}"/>.
        /// </summary>
        /// <param name="values">Array of value.</param>
        /// <param name="poolIndex">Pool index.</param>
        public ValuePool(
            IEnumerable<TValue> values,
            int poolIndex = 0)
        {
            _values.TryAdd(poolIndex, values.Where(x => (x is string str && !string.IsNullOrWhiteSpace(str)) || x is not string).ToList());
            _currentValueIndex.TryAdd(poolIndex, 0);
        }

        /// <inheritdoc />
        public TValue GetNextValue(
            int poolIndex = 0)
        {
            lock (_values)
            {
                if (!_values.ContainsKey(poolIndex) || _values[poolIndex].Count == 0)
                {
                    return default!;
                }

                TValue value;
                bool shouldGetNext;
                do
                {
                    value = _values[poolIndex][_currentValueIndex[poolIndex]];
                    _currentValueIndex[poolIndex] = (_currentValueIndex[poolIndex] + 1) % _values[poolIndex].Count;
                    shouldGetNext = value.Equals(default);
                }
                while (shouldGetNext);

                return value!;
            }
        }

        /// <inheritdoc />
        public int CurrentIndex(
            int poolIndex = 0)
        {
            lock (_values)
            {
                return _currentValueIndex[poolIndex];
            }
        }
    }

    /// <inheritdoc cref="IValuePool{TService, TValue}"/>
    public class ValuePool<TValue> :
        ValuePool<ValuePool<TValue>, TValue>
        where TValue : class
    {
        /// <summary>
        /// Initialize <see cref="ValuePool{TService, TValue}"/>.
        /// </summary>
        /// <param name="values">Array of value.</param>
        /// <param name="poolIndex">Pool index.</param>
        public ValuePool(
            IEnumerable<TValue> values,
            int poolIndex = 0)
            : base(values, poolIndex)
        {
        }
    }
}