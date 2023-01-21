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

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenHolidaysApi.DataLayer
{
    /// <summary>
    /// Representation of a subdivision (e.g. a federal state or a canton)
    /// </summary>
    [Table(DbTables.Subdivision)]
    [Index(nameof(Code), IsUnique = true)]
    [Index(nameof(CountryId), nameof(ShortName), IsUnique = true)]
    [Comment("Representation of a subdivision (e.g. a federal state or a canton)")]
    public class Subdivision : BaseEntity
    {
        /// <summary>
        /// Localized categories
        /// </summary>
        [Required]
        [Column(TypeName = "jsonb")]
        [Comment("Localized categories")]
        public ICollection<LocalizedText> Category { get; set; } = new List<LocalizedText>();

        /// <summary>
        /// List of subdivision children
        /// </summary>
        public virtual ICollection<Subdivision> Children { get; set; } = new List<Subdivision>();

        /// <summary>
        /// Subdivision code
        /// </summary>
        [Required]
        [Comment("Subdivision code")]
        public string Code { get; set; }

        /// <summary>
        /// Additional localized comments
        /// </summary>
        [Column(TypeName = "jsonb")]
        [Comment("Additional localized comments")]
        public ICollection<LocalizedText> Comment { get; set; } = new List<LocalizedText>();

        /// <summary>
        /// Reference to country
        /// </summary>
        [Required]
        public Country Country { get; set; }

        /// <summary>
        /// List of holidays
        /// </summary>
        public virtual ICollection<Holiday> Holidays { get; set; } = new List<Holiday>();

        /// <summary>
        /// Subdivision ISO 3166-2 code (if available)
        /// </summary>
        [Comment("Subdivision ISO 3166-2 code (if available)")]
        public string IsoCode { get; set; }

        /// <summary>
        /// Localized subdivision names 
        /// </summary>
        [Required]
        [Column(TypeName = "jsonb")]
        [Comment("Localized subdivision names")]
        public ICollection<LocalizedText> Name { get; set; } = new List<LocalizedText>();

        /// <summary>
        /// Official languages as ISO-639-1 codes
        /// </summary>
        [Required]
        [Column(TypeName = "jsonb")]
        [Comment("Official languages as ISO-639-1 codes")]
        public ICollection<string> OfficialLanguages { get; set; } = new List<string>();

        /// <summary>
        /// Code of parent subdivision
        /// </summary>
        public Subdivision Parent { get; set; }

        /// <summary>
        /// Short name for display
        /// </summary>
        [Required]
        [Comment("Short name for display")]
        public string ShortName { get; set; }

        #region Foreign keys
        [Comment("Reference to country")]
        public Guid CountryId { get; set; }
        [Comment("Reference to parent subdivision")]
        public Guid? ParentId { get; set; }
        #endregion Foreign keys
    }
}

