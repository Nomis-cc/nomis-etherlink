// ------------------------------------------------------------------------------------------------------
// <copyright file="CmsServiceSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------

using Nomis.Utils.Contracts.Common;

namespace Nomis.CmsService.Settings
{
    /// <summary>
    /// Cms service settings.
    /// </summary>
    public class CmsServiceSettings :
        ISettings
    {
        /// <summary>
        /// Base URL of CMS API.
        /// </summary>
        public string BaseUrl { get; init; } = null!;

        /// <summary>
        /// CMS API KEY.
        /// </summary>
        public string ApiKey { get; init; } = null!;
    }
}