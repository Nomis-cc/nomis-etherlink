// ------------------------------------------------------------------------------------------------------
// <copyright file="INomisHoldersService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.NomisHolders.Interfaces.Enums;
using Nomis.NomisHolders.Interfaces.Models;
using Nomis.Utils.Contracts.Services;

namespace Nomis.NomisHolders.Interfaces
{
    /// <summary>
    /// Nomis holders service.
    /// </summary>
    public interface INomisHoldersService :
        IInfrastructureService
    {
        /// <summary>
        /// Get holder data for wallet address and giving score.
        /// </summary>
        /// <param name="score">Nomis score.</param>
        /// <param name="address">Wallet address.</param>
        /// <param name="useSubgraph">Use subgraph.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>Returns holder data for wallet address and giving score.</returns>
        public Task<NomisHoldersData> HolderAsync(
            NomisHoldersScore score,
            string address,
            bool useSubgraph = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get holders data for wallets addresses and giving score.
        /// </summary>
        /// <param name="score">Nomis score.</param>
        /// <param name="addresses">Wallet addresses.</param>
        /// <param name="useSubgraph">Use subgraph.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>Returns holders data for wallets addresses and giving score.</returns>
        public Task<IList<NomisHoldersData>> HoldersAsync(
            NomisHoldersScore score,
            IList<string> addresses,
            bool useSubgraph = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get wallet social account.
        /// </summary>
        /// <param name="address">Wallet address.</param>
        /// <param name="provider">Social account provider.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>Returns wallet social account.</returns>
        public Task<NomisWalletSocialAccount> WalletSocialAccountAsync(
            string address,
            NomisSocialAccountProvider provider,
            CancellationToken cancellationToken = default);
    }
}