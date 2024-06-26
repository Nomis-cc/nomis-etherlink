﻿// ------------------------------------------------------------------------------------------------------
// <copyright file="Cms.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomis.CmsService.Extensions;
using Nomis.CmsService.Interfaces;
using Nomis.Utils.Contracts.Services;

namespace Nomis.CmsService
{
    /// <summary>
    /// CMS service registrar.
    /// </summary>
    public sealed class Cms :
        ICmsServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddCmsService(configuration);
        }

        /// <inheritdoc/>
        public IInfrastructureService GetService(
            IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<ICmsService>();
        }
    }
}