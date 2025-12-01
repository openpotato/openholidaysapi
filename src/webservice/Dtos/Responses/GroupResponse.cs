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

using OpenHolidaysApi.DataLayer;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenHolidaysApi
{
    /// <summary>
    /// Representation of a holiday zone
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class GroupResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupResponse"/> class.
        /// </summary>
        /// <param name="group">Assigns data from <see cref="Group"/></param>
        /// <param name="languageCode">Language code or null</param>
        public GroupResponse(Group group, string languageCode)
        {
            Code = group.Code;
            ShortName = group.ShortName;
            Category = group.Category.ToLocalizedList(languageCode);
            Name = group.Name.ToLocalizedList(languageCode);
            Subdivisions = [.. group.Subdivisions.Select(x => new SubdivisionReference() { Code = x.Code, ShortName = x.ShortName })];
            Comment = group.Comment.ToLocalizedList(languageCode);
            Children = group.Children.ToResponseList(languageCode);
        }

        /// <summary>
        /// Localized categories of the group
        /// </summary>
        /// <example>[{"language":"fr","text":"zone"},{"language":"de","text":"Zone"}]</example>
        [Required]
        [JsonPropertyOrder(3)]
        public List<LocalizedText> Category { get; set; }

        /// <summary>
        /// Child groups
        /// </summary>
        [JsonPropertyOrder(7)]
        public List<GroupResponse> Children { get; set; }

        /// <summary>
        /// Group code 
        /// </summary>
        /// <example>FR-ZA</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string Code { get; set; }

        /// <summary>
        /// Localized comments of the group
        /// </summary>
        /// <example>null</example>
        [Required]
        [JsonPropertyOrder(6)]
        public List<LocalizedText> Comment { get; set; }

        /// <summary>
        /// Localized names of the group
        /// </summary>
        /// <example>[{"language":"fr","text":"Zone A"},{"language":"de","text":"Zone A"}]</example>
        [Required]
        [JsonPropertyOrder(4)]
        public List<LocalizedText> Name { get; set; }

        /// <summary>
        /// Short name for display
        /// </summary>
        /// <example>ZA</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string ShortName { get; set; }

        /// <summary>
        /// List of subdivision references
        /// </summary>
        /// <example>>["FR-BF-TB"]</example>
        [JsonPropertyOrder(5)]
        public List<SubdivisionReference> Subdivisions { get; set; }
    }
}
