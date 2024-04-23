// ------------------------------------------------------------------------------------------------------
// <copyright file="ReferralsController.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Mime;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nomis.Api.Common.Swagger.Examples;
using Nomis.NomisHolders.Interfaces.Enums;
using Nomis.ReferralService.Interfaces;
using Nomis.ReferralService.Interfaces.Contracts;
using Nomis.ReferralService.Interfaces.Requests;
using Nomis.Utils.Contracts.Referrals;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Referrals
{
    /// <summary>
    /// A controller to aggregate all Referrals-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Referrals service.")]
    public sealed class ReferralsController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/referrals";

        /// <summary>
        /// Common tag for Referrals actions.
        /// </summary>
        internal const string ReferralsTag = "Referrals";

        private readonly ILogger<ReferralsController> _logger;
        private readonly IReferralService _referralsService;

        /// <summary>
        /// Initialize <see cref="ReferralsController"/>.
        /// </summary>
        /// <param name="referralsService"><see cref="IReferralService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public ReferralsController(
            IReferralService referralsService,
            ILogger<ReferralsController> logger)
        {
            _referralsService = referralsService ?? throw new ArgumentNullException(nameof(referralsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get referrals data with referral level by referrer wallet.
        /// </summary>
        /// <param name="score">Nomis score.</param>
        /// <param name="walletAddress">Wallet address.</param>
        /// <param name="maxReferralLevel">Max referral level to retrieve.</param>
        /// <returns>Returns referrals data with referral level by referrer wallet.</returns>
        /// <response code="200">Returns referrals data with referral level by referrer wallet.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("by-level", Name = "GetReferralsByLevel")]
        [SwaggerOperation(
            OperationId = "GetReferralsByLevel",
            Tags = new[] { ReferralsTag })]
        [ProducesResponseType(typeof(Result<Dictionary<int, ReferralsData>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetReferralsWithLevelByReferrerWalletAsync(
            NomisHoldersScore score,
            string walletAddress,
            int maxReferralLevel = 1)
        {
            var result = await _referralsService.GetReferralsWithLevelByReferrerWalletAsync(score, walletAddress, maxReferralLevel).ConfigureAwait(false);
            return Ok(result);
        }

        /// <summary>
        /// Get referrals data with referral level by referrer wallets.
        /// </summary>
        /// <param name="request">Request for retrieving data.</param>
        /// <returns>Returns referrals data with referral level by referrer wallets.</returns>
        /// <response code="200">Returns referrals data with referral level by referrer wallets.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpPost("all-by-level", Name = "GetAllReferralsByLevel")]
        [SwaggerOperation(
            OperationId = "GetAllReferralsByLevel",
            Tags = new[] { ReferralsTag })]
        [ProducesResponseType(typeof(Result<List<ReferralsWithLevelData>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetReferralsWithLevelByReferrerWalletsAsync(
            [FromBody] ReferralsWithLevelByReferrerWalletsRequest request)
        {
            var result = await _referralsService.GetReferralsWithLevelByReferrerWalletsAsync(request).ConfigureAwait(false);
            return Ok(result);
        }
    }
}