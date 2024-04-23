// ------------------------------------------------------------------------------------------------------
// <copyright file="IEtherlinkExplorerClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Clients;

namespace Nomis.EtherlinkExplorer.Interfaces
{
    /// <summary>
    /// Etherlink Explorer client.
    /// </summary>
    public interface IEtherlinkExplorerClient :
        IBaseEvmClient
    {
    }
}