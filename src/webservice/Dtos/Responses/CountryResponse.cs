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
    /// Representation of a country as defined in ISO 3166-1 
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class CountryResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountryResponse"/> class.
        /// </summary>
        /// <param name="country">Assigns data from <see cref="Country"/></param>
        /// <param name="languageCode">Language code or null</param>
        public CountryResponse(Country country, string languageCode)
        {
            IsoCode = country.IsoCode;
            Name = country.Name.ToLocalizedList(languageCode);
            OfficialLanguages = country.OfficialLanguages.ToList();
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
        public List<LocalizedText> Name { get; set; }

        /// <summary>
        /// Official ISO-639-1 language codes
        /// </summary>
        /// <example>["DE"]</example>
        [Required]
        [JsonPropertyOrder(4)]
        public List<string> OfficialLanguages { get; set; }
    }
}
