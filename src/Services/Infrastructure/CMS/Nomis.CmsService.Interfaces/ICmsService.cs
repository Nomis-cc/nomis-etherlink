// ------------------------------------------------------------------------------------------------------
// <copyright file="ICmsService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.CmsService.Interfaces.Enums;
using Nomis.Utils.Contracts.Services;
using Nomis.Utils.Wrapper;

namespace Nomis.CmsService.Interfaces;

/// <summary>
/// CMS service.
/// </summary>
public interface ICmsService :
    ISingletonService,
    IInfrastructureService
{
    /// <summary>
    /// Get account data by social account.
    /// </summary>
    /// <param name="provider">Social account provider.</param>
    /// <param name="username">Social account username.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    Task<Result<IDictionary<string, string?>>> AccountDataBySocialAccountAsync(
        NomisCmsSocialAccountProvider provider,
        string username,
        CancellationToken cancellationToken);
}