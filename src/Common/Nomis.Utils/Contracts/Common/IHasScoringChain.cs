// ------------------------------------------------------------------------------------------------------
// <copyright file="IHasScoringChain.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Utils.Enums;

namespace Nomis.Utils.Contracts.Common
{
    /// <summary>
    /// Has properties for scoring chain.
    /// </summary>
    public interface IHasScoringChain
    {
        /// <summary>
        /// Scoring calculation model.
        /// </summary>
        /// <example>11</example>
        public ScoringCalculationModel CalculationModel { get; set; }

        /// <summary>
        /// Score type.
        /// </summary>
        /// <example>0</example>
        public ScoreType ScoreType { get; set; }

        /// <summary>
        /// Blockchain type in which the score will be calculated.
        /// </summary>
        /// <example>0</example>
        public ScoringChainType ScoringChainType { get; set; }
    }
}