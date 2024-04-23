// ------------------------------------------------------------------------------------------------------
// <copyright file="NomisHoldersService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.Extensions.Logging;
using Nomis.NomisHolders.Interfaces;
using Nomis.NomisHolders.Interfaces.Enums;
using Nomis.NomisHolders.Interfaces.Models;
using Nomis.NomisHolders.Interfaces.Requests;
using Nomis.NomisHolders.Settings;
using Nomis.Utils.Contracts.Services;
using Nomis.Utils.Converters;
using Nomis.Utils.Exceptions;
using Nomis.Utils.Extensions;
using Nomis.Utils.Wrapper;

namespace Nomis.NomisHolders
{
    /// <inheritdoc cref="INomisHoldersService"/>
    internal sealed class NomisHoldersService :
        INomisHoldersService,
        ISingletonService,
        IDisposable
    {
        private readonly NomisHoldersSettings _settings;
        private readonly HttpClient _client;
        private readonly ILogger<NomisHoldersService> _logger;
        private readonly Dictionary<NomisHoldersScore, NomisHoldersSubgraphGraphQLClient> _graphQlClients;

        /// <summary>
        /// Initialize <see cref="NomisHoldersService"/>.
        /// </summary>
        /// <param name="settings"><see cref="NomisHoldersSettings"/>.</param>
        /// <param name="client"><see cref="HttpClient"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public NomisHoldersService(
            NomisHoldersSettings settings,
            HttpClient client,
            ILogger<NomisHoldersService> logger)
        {
            _settings = settings;
            _client = client;
            _logger = logger;

            _graphQlClients = new Dictionary<NomisHoldersScore, NomisHoldersSubgraphGraphQLClient>();
            foreach (var chain in Enum.GetValues<NomisHoldersScore>())
            {
                if (settings.SubgraphsApis.TryGetValue(chain, out string? subgraphApiUrl))
                {
                    var graphQlOptions = new GraphQLHttpClientOptions
                    {
                        EndPoint = new(subgraphApiUrl)
                    };
                    _graphQlClients.Add(chain, new NomisHoldersSubgraphGraphQLClient(graphQlOptions, new SystemTextJsonSerializer()));
                }
            }
        }

        /// <inheritdoc />
        public async Task<NomisHoldersData> HolderAsync(
            NomisHoldersScore score,
            string address,
            bool useSubgraph = true,
            CancellationToken cancellationToken = default)
        {
            if (score == NomisHoldersScore.None)
            {
                return new NomisHoldersData
                {
                    Message = "Nomis score should be set."
                };
            }

            if (_settings.UseSubgraphs && useSubgraph && _graphQlClients.TryGetValue(score, out var graphQlClient))
            {
                try
                {
                    var graphQlRequest = new NomisHolderDataRequest
                    {
                        Address = address
                    };
                    var nomisHolderSubgraphDataResult = await GetNomisHolderDataAsync(graphQlClient, graphQlRequest).ConfigureAwait(false);
                    if (nomisHolderSubgraphDataResult is { Succeeded: true, Data: not null })
                    {
                        var data = nomisHolderSubgraphDataResult.Data;
                        var res = new NomisHoldersData
                        {
                            Score = data.Score,
                            CalculationModel = data.CalculationModel,
                            ChainId = ulong.TryParse(data.ChainId, out ulong chainId) ? chainId : 0,
                            Owner = data.Owner,
                            TokenId = int.TryParse(data.TokenId, out int tokenId) ? tokenId : 0,
                            Updated = long.TryParse(data.Updated, out long updated) ? updated : null,
                            Message = nomisHolderSubgraphDataResult.Messages.FirstOrDefault(),
                            IsHolder = data.Score > 0,
                            Version = "0.8"
                        };
                        return res;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "{Service} ({Score}): There is an error while fetching wallet holder data from subgraph", nameof(NomisHoldersService), score.ToString());
                }
            }

            string request =
                $"/api/{score.ToDescriptionString()}/holder?address={address}";

            var response = await _client.GetAsync(request, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("{Service} ({Score}): There is an error while fetching wallet holder data with status code {StatusCode}: {Response}", nameof(NomisHoldersService), score.ToString(), response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false));
                return new NomisHoldersData
                {
                    Message = $"There is an error while fetching wallet holder data for {score.ToString()} score."
                };
            }

            var result = await response.Content.ReadFromJsonAsync<NomisHoldersData>(cancellationToken: cancellationToken).ConfigureAwait(false)
                   ?? throw new CustomException($"Can't get wallet holder data for {score.ToString()} score.");
            result.Score *= 100;

            return result;
        }

        /// <inheritdoc />
        public async Task<IList<NomisHoldersData>> HoldersAsync(
            NomisHoldersScore score,
            IList<string> addresses,
            bool useSubgraph = true,
            CancellationToken cancellationToken = default)
        {
            return await Task.WhenAll(addresses.Select(x => HolderAsync(score, x, useSubgraph, cancellationToken))).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<NomisWalletSocialAccount> WalletSocialAccountAsync(
            string address,
            NomisSocialAccountProvider provider,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _client.Dispose();
        }

        private async Task<Result<NomisHolderSubgraphData?>> GetNomisHolderDataAsync(
            NomisHoldersSubgraphGraphQLClient client,
            NomisHolderDataRequest request)
        {
            var query = new GraphQLRequest
            {
                Query = """
                        query($address: Bytes!) {
                          changedScores(
                            where: {
                                owner: $address
                            },
                            orderBy: blockTimestamp,
                            orderDirection: desc,
                            first: 1) {
                            tokenId
                            owner
                            score
                            calculationModel
                            chainId
                            blockTimestamp
                          }
                        }
                        """,
                Variables = request
            };

            var response = await client.SendQueryAsync<JsonObject>(query).ConfigureAwait(false);
            var data = JsonSerializer.Deserialize<List<NomisHolderSubgraphData>>(response.Data["changedScores"]?.ToJsonString(new()
            {
                Converters = { new BigIntegerConverter() }
            }) !);

            return await Result<NomisHolderSubgraphData?>.SuccessAsync(data?.FirstOrDefault(), "Nomis holder data received.").ConfigureAwait(false);
        }
    }
}