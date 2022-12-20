#region OpenHolidays API - Copyright (C) 2022 STÜBER SYSTEMS GmbH
/*    
 *    OpenHolidays API 
 *    
 *    Copyright (C) 2022 STÜBER SYSTEMS GmbH
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
    /// Representation of a country as defined in ISO 3166-1 
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class CountryResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountryResponse"/> class.
        /// </summary>
        /// <param name="country">Assigns data from <see cref="Country"/></param>
        public CountryResponse(Country country)
        {
            IsoCode = country.IsoCode;
            Names = country.Names.Select(x => new LocalizedText { Language = x.Language, Text = x.Text }).ToList();
            OfficialNames = country.OfficialNames.Select(x => new LocalizedText { Language = x.Language, Text = x.Text }).ToList(); 
            OfficialLanguages = country.OfficialLanguages;
        }

        /// <summary>
        /// ISO 3166-1 country code
        /// </summary>
        /// <example>DE</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string IsoCode { get; set; }

        /// <summary>
        /// Localized country names
        /// </summary>
        /// <example>[{"language":"EN","text":"Germany"},{"language":"DE","text":"Deutschland"}]</example>
        [Required]
        [JsonPropertyOrder(2)]
        public ICollection<LocalizedText> Names { get; set; }

        /// <summary>
        /// Official ISO-639-1 language codes
        /// </summary>
        /// <example>["DE"]</example>
        [Required]
        [JsonPropertyOrder(4)]
        public ICollection<string> OfficialLanguages { get; set; }

        /// <summary>
        /// Official ISO-3166-1 country names
        /// </summary>
        /// <example>[{"language":"EN","text":"The Federal Republic of Germany"},{"language":"DE","text":"Bundesrepublik Deutschland"}]</example>
        [Required]
        [JsonPropertyOrder(3)]
        public ICollection<LocalizedText> OfficialNames { get; set; }
    }
}
