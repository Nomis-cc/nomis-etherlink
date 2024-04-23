// ------------------------------------------------------------------------------------------------------
// <copyright file="ReferralsDataExtended.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

using Nomis.NomisHolders.Interfaces.Models;
using Nomis.Utils.Contracts.Referrals;

namespace Nomis.ReferralService.Interfaces.Contracts
{
    /// <summary>
    /// Referrals data extended.
    /// </summary>
    public class ReferralsDataExtended :
        ReferralsData
    {
        /// <summary>
        /// Holder data.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public NomisHoldersData? HolderData { get; set; }
    }
}