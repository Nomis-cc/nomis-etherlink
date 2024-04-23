// ------------------------------------------------------------------------------------------------------
// <copyright file="NomisHoldersExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.NomisHolders.Settings;
using Nomis.NomisHolders.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.NomisHolders.Extensions
{
    /// <summary>
    /// Nomis holders extension methods.
    /// </summary>
    public static class NomisHoldersExtensions
    {
        /// <summary>
        /// Add Nomis holders API.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        // ReSharper disable once InconsistentNaming
        public static IScoringOptionsBuilder WithNomisHoldersAPI<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : INomisHoldersServiceRegistrar, new()
        {
            return optionsBuilder
                .With<NomisHoldersAPISettings, TServiceRegistrar>();
        }
    }
}