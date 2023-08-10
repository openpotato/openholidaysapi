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
    /// Representation of a holiday by date 
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class HolidayByDateResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HolidayByDateResponse"/> class.
        /// </summary>
        /// <param name="holiday">Assigns data from <see cref="Holiday"/></param>
        /// <param name="languageCode">Language code or null</param>
        public HolidayByDateResponse(Holiday holiday, string languageCode)
        {
            Id = holiday.Id;
            Country = new CountryReference() { IsoCode = holiday.Country.IsoCode };
            Type = (HolidayType)holiday.Type;
            Nationwide = holiday.Nationwide;
            Subdivisions = holiday.Subdivisions.Select(x => new SubdivisionReference() { Code = x.Code, ShortName = x.ShortName }).ToList();
            Name = holiday.Name.ToLocalizedList(languageCode);
            Comment = holiday.Comment.ToLocalizedList(languageCode);
        }

        /// <summary>
        /// Additional localized comments
        /// </summary>
        [JsonPropertyOrder(6)]
        public List<LocalizedText> Comment { get; set; }

        /// <summary>
        /// Country references
        /// </summary>
        [Required]
        [JsonPropertyOrder(1)]
        public CountryReference Country { get; set; }

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
        [JsonPropertyOrder(2)]
        public bool Nationwide { get; set; }

        /// <summary>
        /// List of subdivision references
        /// </summary>
        [Required]
        [JsonPropertyOrder(3)]
        public List<SubdivisionReference> Subdivisions { get; set; }

        /// <summary>
        /// Type of holiday
        /// </summary>
        /// <example>Public</example>
        [Required]
        [JsonPropertyOrder(4)]
        public HolidayType Type { get; set; }
    }
}