// ------------------------------------------------------------------------------------------------------
// <copyright file="INomisHoldersSubgraphGraphQLClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using GraphQL.Client.Abstractions;

// ReSharper disable InconsistentNaming
namespace Nomis.NomisHolders.Interfaces
{
    /// <summary>
    /// Nomis holders subgraph GraphQL client.
    /// </summary>
    public interface INomisHoldersSubgraphGraphQLClient :
        IGraphQLClient
    {
    }
}