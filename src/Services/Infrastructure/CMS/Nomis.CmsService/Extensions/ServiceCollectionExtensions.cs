// ------------------------------------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomis.CmsService.Interfaces;
using Nomis.CmsService.Settings;
using Nomis.Utils.Extensions;

namespace Nomis.CmsService.Extensions
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add CMS service.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <returns>Returns <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddCmsService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSettings<CmsServiceSettings>(configuration);
            var settings = configuration.GetSettings<CmsServiceSettings>();
            services.AddHttpClient<CmsClient>(client =>
            {
                client.BaseAddress = new Uri(settings.BaseUrl ?? "https://cms.nomis.cc/");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {settings.ApiKey}");
            });

            return services.AddSingletonInfrastructureService<ICmsService, CmsService>();
        }
    }
}