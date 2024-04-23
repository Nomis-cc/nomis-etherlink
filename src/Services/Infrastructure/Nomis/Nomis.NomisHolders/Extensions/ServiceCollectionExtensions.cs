// ------------------------------------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nomis.Blockchain.Abstractions.Extensions;
using Nomis.NomisHolders.Interfaces;
using Nomis.NomisHolders.Settings;
using Nomis.Utils.Extensions;

namespace Nomis.NomisHolders.Extensions
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Nomis holders API service.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <returns>Returns <see cref="IServiceCollection"/>.</returns>
        // ReSharper disable once InconsistentNaming
        public static IServiceCollection AddNomisHoldersService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSettings<NomisHoldersSettings>(configuration);
            var settings = configuration.GetSettings<NomisHoldersSettings>();
            services
                .AddHttpClient<NomisHoldersService>(client =>
                {
                    client.BaseAddress = new(settings.ApiBaseUrl ?? "https://nomis.cc/");
                })
                .AddRateLimitHandler(settings);

            return services
                .AddSingleton<INomisHoldersService, NomisHoldersService>(provider =>
                {
                    var logger = provider.GetRequiredService<ILogger<NomisHoldersService>>();
                    var client = provider.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(NomisHoldersService));
                    return new NomisHoldersService(settings, client, logger);
                });
        }
    }
}