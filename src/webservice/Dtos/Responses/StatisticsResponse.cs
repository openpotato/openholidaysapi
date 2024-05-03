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
    /// Statistical data of the holidays database
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class StatisticsResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsResponse"/> class.
        /// </summary>
        /// <param name="youngestStartDate">The youngest start date in the database</param>
        /// <param name="oldestStartDate">The oldest start date in the database</param>
        public StatisticsResponse(DateOnly youngestStartDate, DateOnly oldestStartDate)
        {
            YoungestStartDate = youngestStartDate;
            OldestStartDate = oldestStartDate;
        }

        /// <summary>
        /// The youngest holiday start date in the database
        /// </summary>
        [Required]
        [JsonPropertyOrder(1)]
        public DateOnly YoungestStartDate { get; set; }

        /// <summary>
        /// The oldest holiday start date in the database
        /// </summary>
        [Required]
        [JsonPropertyOrder(2)]
        public DateOnly OldestStartDate { get; set; }
    }
}
