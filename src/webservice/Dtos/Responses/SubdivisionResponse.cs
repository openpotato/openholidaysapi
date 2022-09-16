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
    /// Representation of a subdivision as defined in ISO 3166-2
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class SubdivisionResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubdivisionResponse"/> class.
        /// </summary>
        /// <param name="subdivision">Assigns data from <see cref="Subdivision"/></param>
        public SubdivisionResponse(Subdivision subdivision)
        {
            IsoCode = subdivision.IsoCode;
            Names = subdivision.Names.Select(x => new LocalizedText { Language = x.Language, Text = x.Text }).ToList();
            OfficialLanguages = subdivision.OfficialLanguages;
            ShortName = subdivision.ShortName;
            Parent = subdivision.Parent?.IsoCode;
            Comments = subdivision.Comments.Select(x => new LocalizedText { Language = x.Language, Text = x.Text }).ToList();
        }

        /// <summary>
        /// ISO 3166-2 subdivision code
        /// </summary>
        /// <example>DE-BE</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string IsoCode { get; set; }

        /// <summary>
        /// Localized comments of the subdivision
        /// </summary>
        /// <example>null</example>
        [Required]
        [JsonPropertyOrder(6)]
        public ICollection<LocalizedText> Comments { get; set; }

        /// <summary>
        /// Localized names of the subdivision
        /// </summary>
        /// <example>[{"language":"DE","text":"Berlin"},{"language":"EN","text":"Berlin"}]</example>
        [Required]
        [JsonPropertyOrder(3)]
        public ICollection<LocalizedText> Names { get; set; }

        /// <summary>
        /// Official languages as ISO-639-1 codes
        /// </summary>
        /// <example>>["DE"]</example>
        [Required]
        [JsonPropertyOrder(4)]
        public ICollection<string> OfficialLanguages { get; set; }

        /// <summary>
        /// Parent subdivision
        /// </summary>
        /// <example>null</example>
        [JsonPropertyOrder(5)]
        public string Parent { get; set; }

        /// <summary>
        /// Short name for display
        /// </summary>
        /// <example>BE</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string ShortName { get; set; }
    }
}
