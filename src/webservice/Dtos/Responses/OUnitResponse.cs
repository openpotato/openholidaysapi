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
    /// Representation of an organizational unit (e.g. an holiday zone or a school type)
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class OUnitResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OUnitResponse"/> class.
        /// </summary>
        /// <param name="oUnit">Assigns data from <see cref="OUnit"/></param>
        /// <param name="languageCode">Language code or null</param>
        public OUnitResponse(OUnit oUnit, string languageCode)
        {
            Code = oUnit.Code;
            ShortName = oUnit.ShortName;
            Names = oUnit.Names.ToLocalizedList(languageCode);
            Subdivisions = oUnit.Subdivisions.Select(x => new SubdivisionReference() { IsoCode = x.IsoCode, ShortName = x.ShortName }).ToList();
            Parent = oUnit.Parent?.Code;
            Comments = oUnit.Comments.ToLocalizedList(languageCode);
        }

        /// <summary>
        /// Code
        /// </summary>
        /// <example>FR-ZA</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string Code { get; set; }

        /// <summary>
        /// Localized comments of the organizational unit
        /// </summary>
        /// <example>null</example>
        [Required]
        [JsonPropertyOrder(6)]
        public ICollection<LocalizedText> Comments { get; set; }

        /// <summary>
        /// Localized names of the organizational unit
        /// </summary>
        /// <example>[{"language":"FR","text":"Zone A"},{"language":"DE","text":"Zone A"},{"language":"EN","text":"Zone A"}]</example>
        [Required]
        [JsonPropertyOrder(3)]
        public ICollection<LocalizedText> Names { get; set; }

        /// <summary>
        /// Parent ounit
        /// </summary>
        /// <example>null</example>
        [JsonPropertyOrder(5)]
        public string Parent { get; set; }

        /// <summary>
        /// Short name for display
        /// </summary>
        /// <example>ZA</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string ShortName { get; set; }

        /// <summary>
        /// List of subdivisions 
        /// </summary>
        /// <example>null</example>
        [JsonPropertyOrder(4)]
        public virtual ICollection<SubdivisionReference> Subdivisions { get; set; }
    }
}
