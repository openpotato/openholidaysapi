#region OpenHolidays API - Copyright (C) STÜBER SYSTEMS GmbH
/*    
 *    OpenHolidays API 
 *    
 *    Copyright (C) STÜBER SYSTEMS GmbH
 *
 *    This program is free software: you can redistribute it and/or modify
 *    it under the terms of the GNU Affero General Public License, version 3,
 *    as published by the Free Software Foundation.
 *
 *    This program is distributed in the hope that it will be useful,
 *    but WITHOUT ANY WARRANTY; without even the implied warranty of
 *    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *    GNU Affero General Public License for more details.
 *
 *    You should have received a copy of the GNU Affero General Public License
 *    along with this program. If not, see <http://www.gnu.org/licenses/>.
 *
 */
#endregion

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenHolidaysApi
{
    /// <summary>
    /// Representation of a subdivision reference 
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class SubdivisionReference
    {
        /// <summary>
        /// Subdivision code
        /// </summary>
        /// <example>DE-BE</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string Code { get; set; }

        /// <summary>
        /// Short name for display
        /// </summary>
        /// <example>BE</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string ShortName { get; set; }
    }
}