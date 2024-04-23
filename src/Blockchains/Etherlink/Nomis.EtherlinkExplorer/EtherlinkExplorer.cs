// ------------------------------------------------------------------------------------------------------
// <copyright file="EtherlinkExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomis.EtherlinkExplorer.Extensions;
using Nomis.EtherlinkExplorer.Interfaces;
using Nomis.Utils.Contracts.Services;

namespace Nomis.EtherlinkExplorer
{
    /// <summary>
    /// Etherlink Explorer service registrar.
    /// </summary>
    public sealed class EtherlinkExplorer :
        IEtherlinkServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddEtherlinkExplorerService(configuration);
        }

        /// <inheritdoc/>
        public IInfrastructureService GetService(
            IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IEtherlinkScoringService>();
        }
    }
}