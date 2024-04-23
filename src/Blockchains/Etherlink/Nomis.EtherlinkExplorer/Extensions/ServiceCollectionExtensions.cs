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
using Nomis.DexProviderService.Interfaces;
using Nomis.EtherlinkExplorer.Interfaces;
using Nomis.EtherlinkExplorer.Settings;
using Nomis.ProxyService.Interfaces;
using Nomis.Utils.Contracts;
using Nomis.Utils.Contracts.Proxy;
using Nomis.Utils.Enums;
using Nomis.Utils.Extensions;

namespace Nomis.EtherlinkExplorer.Extensions
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Etherlink Explorer service.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <returns>Returns <see cref="IServiceCollection"/>.</returns>
        internal static IServiceCollection AddEtherlinkExplorerService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.CheckServiceDependencies(typeof(EtherlinkExplorerService), typeof(IDexProviderService));
            services.AddSettings<EtherlinkExplorerSettings>(configuration);
            var settings = configuration.GetSettings<EtherlinkExplorerSettings>();

            foreach (ScoringChainType scoreChainType in Enum.GetValuesAsUnderlyingType<ScoringChainType>())
            {
                if (settings.ScoringSettings.TryGetValue(scoreChainType, out var scoringSettings))
                {
                    if (scoringSettings.UseProxies)
                    {
                        var serviceProvider = services.BuildServiceProvider();
                        var proxyService = serviceProvider.GetRequiredService<IProxyService>();
                        foreach (var httpClientProxy in scoringSettings.HttpClientProxies)
                        {
                            var proxyResult = proxyService.GetWebProxy(httpClientProxy.ProxyId);
                            services
                                .AddHttpClient($"{nameof(EtherlinkExplorerService)}_{scoreChainType.ToString()}_{httpClientProxy.ProxyId}", client =>
                                {
                                    client.BaseAddress = new(scoringSettings.ApiBaseUrl ?? (scoringSettings.IsTestnet ? "https://testnet-explorer.etherlink.com/" : "https://explorer.etherlink.com/"));
                                })
                                .ConfigurePrimaryHttpMessageHandler(() => httpClientProxy.ProxyUri == null
                                    ? new HttpClientHandler { UseProxy = false, ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator }
                                    : new HttpClientHandler { Proxy = proxyResult.Data, UseProxy = true, ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator })
                                .AddRateLimitHandler($"{nameof(EtherlinkExplorerService)}_{scoreChainType.ToString()}_{httpClientProxy.ProxyId}");

                            httpClientProxy.ApiKeysPool = new ValuePool<string>(httpClientProxy.ApiKeys);
                            if (proxyResult.Succeeded)
                            {
                                httpClientProxy.ProxyUri = proxyResult.Data.Address;
                            }
                        }

                        services
                            .AddSingleton<IValuePool<EtherlinkExplorerService, HttpClientProxy>>(_ => new ValuePool<EtherlinkExplorerService, HttpClientProxy>(scoringSettings.HttpClientProxies, (int)scoreChainType));

                        if (scoringSettings.IsTestnet)
                        {
                            services
                                .AddTransient<IEtherlinkExplorerTestnetClient, EtherlinkExplorerTestnetClient>(provider =>
                                {
                                    var httpClientProxiesPool = provider.GetRequiredService<IValuePool<EtherlinkExplorerService, HttpClientProxy>>();
                                    var httpClientProxy = httpClientProxiesPool.GetHttpClientProxy(provider, (int)scoreChainType);
                                    httpClientProxy.Client = provider.GetRequiredService<IHttpClientFactory>().CreateClient($"{nameof(EtherlinkExplorerService)}_{scoreChainType.ToString()}_{httpClientProxy.ProxyId}");
                                    var logger = provider.GetRequiredService<ILogger<EtherlinkExplorerTestnetClient>>();

                                    return new EtherlinkExplorerTestnetClient(settings, scoringSettings, httpClientProxy.ApiKeysPool!, httpClientProxy.Client, logger, (int)scoreChainType);
                                });
                        }
                        else
                        {
                            services
                                .AddTransient<IEtherlinkExplorerClient, EtherlinkExplorerClient>(provider =>
                                {
                                    var httpClientProxiesPool = provider.GetRequiredService<IValuePool<EtherlinkExplorerService, HttpClientProxy>>();
                                    var httpClientProxy = httpClientProxiesPool.GetHttpClientProxy(provider, (int)scoreChainType);
                                    httpClientProxy.Client = provider.GetRequiredService<IHttpClientFactory>().CreateClient($"{nameof(EtherlinkExplorerService)}_{scoreChainType.ToString()}_{httpClientProxy.ProxyId}");
                                    var logger = provider.GetRequiredService<ILogger<EtherlinkExplorerClient>>();

                                    return new EtherlinkExplorerClient(settings, scoringSettings, httpClientProxy.ApiKeysPool!, httpClientProxy.Client, logger, (int)scoreChainType);
                                });
                        }
                    }
                    else
                    {
                        services
                            .AddSingleton<IValuePool<EtherlinkExplorerService, string>>(_ => new ValuePool<EtherlinkExplorerService, string>(scoringSettings.ApiKeys));

                        if (scoringSettings.IsTestnet)
                        {
                            services
                                .AddHttpClient<EtherlinkExplorerTestnetClient>(client =>
                                {
                                    client.BaseAddress = new(scoringSettings.ApiBaseUrl ?? "https://testnet-explorer.etherlink.com/");
                                })
                                .AddRateLimitHandler();
                            services
                                .AddTransient<IEtherlinkExplorerTestnetClient, EtherlinkExplorerTestnetClient>(provider =>
                                {
                                    var apiKeysPool = provider.GetRequiredService<IValuePool<EtherlinkExplorerService, string>>();
                                    var logger = provider.GetRequiredService<ILogger<EtherlinkExplorerTestnetClient>>();
                                    var client = provider.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(EtherlinkExplorerTestnetClient));
                                    return new EtherlinkExplorerTestnetClient(settings, scoringSettings, apiKeysPool, client, logger, (int)scoreChainType);
                                });
                        }
                        else
                        {
                            services
                                .AddHttpClient<EtherlinkExplorerClient>(client =>
                                {
                                    client.BaseAddress = new(scoringSettings.ApiBaseUrl ?? "https://explorer.etherlink.com/");
                                })
                                .AddRateLimitHandler();
                            services
                                .AddTransient<IEtherlinkExplorerClient, EtherlinkExplorerClient>(provider =>
                                {
                                    var apiKeysPool = provider.GetRequiredService<IValuePool<EtherlinkExplorerService, string>>();
                                    var logger = provider.GetRequiredService<ILogger<EtherlinkExplorerClient>>();
                                    var client = provider.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(EtherlinkExplorerClient));
                                    return new EtherlinkExplorerClient(settings, scoringSettings, apiKeysPool, client, logger, (int)scoreChainType);
                                });
                        }
                    }
                }
            }

            return services
                .AddTransientInfrastructureService<IEtherlinkScoringService, EtherlinkExplorerService>();
        }
    }
}