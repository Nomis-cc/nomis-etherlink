// ------------------------------------------------------------------------------------------------------
// <copyright file="NomisHoldersApi.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomis.NomisHolders.Extensions;
using Nomis.NomisHolders.Interfaces;
using Nomis.Utils.Contracts.Services;

namespace Nomis.NomisHolders
{
    /// <summary>
    /// Nomis holders API service registrar.
    /// </summary>
    public sealed class NomisHoldersApi :
        INomisHoldersServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddNomisHoldersService(configuration);
        }

        /// <inheritdoc/>
        public IInfrastructureService GetService(
            IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<INomisHoldersService>();
        }
    }
}