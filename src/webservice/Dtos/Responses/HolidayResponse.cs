#region OpenHolidays API - Copyright (C) 2023 STÜBER SYSTEMS GmbH
/*    
 *    OpenHolidays API 
 *    
 *    Copyright (C) 2023 STÜBER SYSTEMS GmbH
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

using OpenHolidaysApi.DataLayer;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenHolidaysApi
{
    /// <summary>
    /// Representation of a holiday
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class HolidayResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HolidayResponse"/> class.
        /// </summary>
        /// <param name="holiday">Assigns data from <see cref="Holiday"/></param>
        /// <param name="languageCode">Language code or null</param>
        public HolidayResponse(Holiday holiday, string languageCode)
        {
            Id = holiday.Id;
            StartDate = holiday.StartDate;
            EndDate = holiday.EndDate;
            Type = (HolidayType)holiday.Type;
            Quality = (HolidayQuality)holiday.Quality;
            Nationwide = holiday.Nationwide;
            Subdivisions = holiday.Subdivisions.Select(x => new SubdivisionReference() { Code = x.Code, ShortName = x.ShortName }).ToList();
            Name = holiday.Name.ToLocalizedList(languageCode);
            Comment = holiday.Comment.ToLocalizedList(languageCode);
        }

        /// <summary>
        /// Additional localized comments
        /// </summary>
        [JsonPropertyOrder(8)]
        public List<LocalizedText> Comment { get; set; }

        /// <summary>
        /// End date of the holiday
        /// </summary>
        /// <example>2022-12-31</example>
        [Required]
        [JsonPropertyOrder(2)]
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// Unqiue holiday id
        /// </summary>
        /// <example>ff3b77a3-8c31-47af-b1c7-f26dd51f3c19</example>
        [Required]
        [JsonPropertyOrder(0)]
        public Guid Id { get; set; }

        /// <summary>
        /// Localized names of the holiday
        /// </summary>
        [Required]
        [JsonPropertyOrder(5)]
        public List<LocalizedText> Name { get; set; }

        /// <summary>
        /// Is the holiday nationwide?
        /// </summary>
        /// <example>true</example>
        [Required]
        [JsonPropertyOrder(6)]
        public bool Nationwide { get; set; }

        /// <summary>
        /// Quality of holiday
        /// </summary>
        /// <example>Mandatory</example>
        [Required]
        [JsonPropertyOrder(4)]
        public HolidayQuality Quality { get; set; }

        /// <summary>
        /// Start date of the holiday
        /// </summary>
        /// <example>2022-01-01</example>
        [Required]
        [JsonPropertyOrder(1)]
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// List of subdivision references
        /// </summary>
        [Required]
        [JsonPropertyOrder(7)]
        public List<SubdivisionReference> Subdivisions { get; set; }

        /// <summary>
        /// Type of holiday
        /// </summary>
        /// <example>Public</example>
        [Required]
        [JsonPropertyOrder(3)]
        public HolidayType Type { get; set; }
    }
}