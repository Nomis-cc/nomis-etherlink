// ------------------------------------------------------------------------------------------------------
// <copyright file="NomisHoldersController.cs" company="Nomis">
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
using Nomis.NomisHolders.Interfaces;
using Nomis.NomisHolders.Interfaces.Enums;
using Nomis.NomisHolders.Interfaces.Models;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.NomisHolders
{
    /// <summary>
    /// A controller to aggregate all Nomis holders-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("NomisHolders API.")]
    public sealed class NomisHoldersController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/nomisholders";

        /// <summary>
        /// Common tag for NomisHolders actions.
        /// </summary>
        internal const string NomisHoldersTag = "NomisHolders";

        private readonly ILogger<NomisHoldersController> _logger;
        private readonly INomisHoldersService _nomisHoldersService;

        /// <summary>
        /// Initialize <see cref="NomisHoldersController"/>.
        /// </summary>
        /// <param name="nomisHoldersExplorerService"><see cref="INomisHoldersService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public NomisHoldersController(
            INomisHoldersService nomisHoldersExplorerService,
            ILogger<NomisHoldersController> logger)
        {
            _nomisHoldersService = nomisHoldersExplorerService ?? throw new ArgumentNullException(nameof(nomisHoldersExplorerService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get holder data for wallet address and giving score.
        /// </summary>
        /// <param name="score" example="1">Blockchain id.</param>
        /// <param name="address">Wallet address.</param>
        /// <param name="useSubgraph">Use subgraph.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Returns holder data for wallet address and giving score.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/v1/nomisholders/data?address=0xfbc44654dd3d5a7a5ea1267928b6e61203856395&amp;score=1
        /// </remarks>
        /// <response code="200">Returns holder data for wallet address and giving score.</response>
        /// <response code="400">Request data not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("data", Name = "GetNomisHoldersData")]
        [SwaggerOperation(
            OperationId = "GetNomisHoldersData",
            Tags = new[] { NomisHoldersTag })]
        [ProducesResponseType(typeof(NomisHoldersData), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> HolderAsync(
            [Required(ErrorMessage = "The score should be set")] NomisHoldersScore score,
            [Required(ErrorMessage = "The wallet address should be set")] string address,
            bool useSubgraph = true,
            CancellationToken cancellationToken = default)
        {
            var result = await _nomisHoldersService.HolderAsync(
                score,
                address,
                useSubgraph,
                cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Get holders data for wallets addresses and giving score.
        /// </summary>
        /// <param name="score" example="1">Blockchain id.</param>
        /// <param name="addresses">Wallets addresses.</param>
        /// <param name="useSubgraph">Use subgraph.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Returns holders data for wallets addresses and giving score.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/v1/nomisholders/multiple-data?addresses=0xeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee,0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48&amp;score=1
        /// </remarks>
        /// <response code="200">Returns holders data for wallets addresses and giving score.</response>
        /// <response code="400">Request data not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("multiple-data", Name = "GetNomisHoldersMultipleData")]
        [SwaggerOperation(
            OperationId = "GetNomisHoldersMultipleData",
            Tags = new[] { NomisHoldersTag })]
        [ProducesResponseType(typeof(List<NomisHoldersData>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> HoldersAsync(
            [Required(ErrorMessage = "The score should be set")] NomisHoldersScore score,
            [Required(ErrorMessage = "The wallets addresses should be set")] IList<string> addresses,
            bool useSubgraph = true,
            CancellationToken cancellationToken = default)
        {
            var result = await _nomisHoldersService.HoldersAsync(
                score,
                addresses,
                useSubgraph,
                cancellationToken);
            return Ok(result);
        }
    }
}