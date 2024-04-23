// ------------------------------------------------------------------------------------------------------
// <copyright file="EtherlinkController.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nomis.Api.Common.Swagger.Examples;
using Nomis.EtherlinkExplorer.Interfaces;
using Nomis.EtherlinkExplorer.Interfaces.Models;
using Nomis.EtherlinkExplorer.Interfaces.Requests;
using Nomis.Utils.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Etherlink
{
    /// <summary>
    /// A controller to aggregate all Etherlink-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Etherlink blockchain.")]
    public sealed class EtherlinkController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/etherlink";

        /// <summary>
        /// Common tag for Etherlink actions.
        /// </summary>
        internal const string EtherlinkTag = "Etherlink";

        private readonly ILogger<EtherlinkController> _logger;
        private readonly IEtherlinkScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="EtherlinkController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IEtherlinkScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public EtherlinkController(
            IEtherlinkScoringService scoringService,
            ILogger<EtherlinkController> logger)
        {
            _scoringService = scoringService ?? throw new ArgumentNullException(nameof(scoringService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get Nomis Score for given wallet address.
        /// </summary>
        /// <param name="request">Request for getting the wallet stats.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>An Nomis Score value and corresponding statistical data.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/v1/etherlink/wallet/0xaA55EFF252336e310edb0B9dF91B81636220462a/score?scoreType=0&amp;nonce=0&amp;deadline=1790647549
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetEtherlinkWalletScore")]
        [SwaggerOperation(
            OperationId = "GetEtherlinkWalletScore",
            Tags = new[] { EtherlinkTag })]
        [ProducesResponseType(typeof(Result<EtherlinkWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetEtherlinkWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] EtherlinkWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<EtherlinkWalletStatsRequest, EtherlinkWalletScore, EtherlinkWalletStats, EtherlinkTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}