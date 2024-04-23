// ------------------------------------------------------------------------------------------------------
// <copyright file="NomisHoldersAPISettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Utils.Contracts.Common;

namespace Nomis.Api.NomisHolders.Settings
{
    /// <summary>
    /// Nomis holders API settings.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    internal class NomisHoldersAPISettings :
        IApiSettings
    {
        /// <inheritdoc/>
        public bool APIEnabled { get; init; }

        /// <inheritdoc/>
        public string APIName => NomisHoldersController.NomisHoldersTag;

        /// <inheritdoc/>
        public string ControllerName => nameof(NomisHoldersController);
    }
}