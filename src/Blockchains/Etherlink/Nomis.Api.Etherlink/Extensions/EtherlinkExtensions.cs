// ------------------------------------------------------------------------------------------------------
// <copyright file="EtherlinkExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Etherlink.Settings;
using Nomis.EtherlinkExplorer.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Etherlink.Extensions
{
    /// <summary>
    /// Etherlink extension methods.
    /// </summary>
    public static class EtherlinkExtensions
    {
        /// <summary>
        /// Add Etherlink blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithEtherlinkBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IEtherlinkServiceRegistrar, new()
        {
            return optionsBuilder
                .With<EtherlinkAPISettings, TServiceRegistrar>();
        }
    }
}