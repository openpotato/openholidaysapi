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
    /// Representation of a language as defined in ISO-639-1
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class LanguageResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageResponse"/> class.
        /// </summary>
        /// <param name="language">Assigns data from <see cref="Language"/></param>
        public LanguageResponse(Language language)
        {
            IsoCode = language.IsoCode;
            Names = language.Names.Select(x => new LocalizedText { Language = x.Language, Text = x.Text }).ToList();
        }

        /// <summary>
        /// ISO-639-1 language code
        /// </summary>
        /// <example>DE</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string IsoCode { get; set; }

        /// <summary>
        /// Localized language names
        /// </summary>
        /// <example>[{"language":"DE","text":"Deutsch"},{"language":"EN","text":"German"}]</example>
        [Required]
        [JsonPropertyOrder(2)]
        public ICollection<LocalizedText> Names { get; set; }
    }
}
