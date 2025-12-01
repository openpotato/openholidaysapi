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

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenHolidaysApi.DataLayer
{
    /// <summary>
    /// Representation of a group (e.g. a holiday zone)
    /// </summary>
    [Table(DbTables.Group)]
    [Index(nameof(Code), IsUnique = true)]
    [Index(nameof(CountryId), nameof(ShortName), IsUnique = true)]
    [Comment("Representation of a group (e.g. a holiday zone)")]
    public class Group : BaseEntity
    {
        /// <summary>
        /// Localized categories
        /// </summary>
        [Required]
        [Column(TypeName = "jsonb")]
        [Comment("Localized categories")]
        public ICollection<LocalizedText> Category { get; set; } = [];

        /// <summary>
        /// List of group children
        /// </summary>
        public virtual ICollection<Group> Children { get; set; } = [];

        /// <summary>
        /// Group code
        /// </summary>
        [Required]
        [Comment("Group code")]
        public string Code { get; set; }

        /// <summary>
        /// Additional localized comments
        /// </summary>
        [Column(TypeName = "jsonb")]
        [Comment("Additional localized comments")]
        public ICollection<LocalizedText> Comment { get; set; } = [];

        /// <summary>
        /// Reference to country
        /// </summary>
        [Required]
        public Country Country { get; set; }

        /// <summary>
        /// List of holidays
        /// </summary>
        public virtual ICollection<Holiday> Holidays { get; set; } = [];

        /// <summary>
        /// Localized group names 
        /// </summary>
        [Required]
        [Column(TypeName = "jsonb")]
        [Comment("Localized group names")]
        public ICollection<LocalizedText> Name { get; set; } = [];

        /// <summary>
        /// Code of parent group
        /// </summary>
        public Group Parent { get; set; }

        /// <summary>
        /// Short name for display
        /// </summary>
        [Required]
        [Comment("Short name for display")]
        public string ShortName { get; set; }

        /// <summary>
        /// List of subdivision references
        /// </summary>
        public virtual ICollection<Subdivision> Subdivisions { get; set; } = [];
        
        #region Foreign keys
        [Comment("Reference to country")]
        public Guid CountryId { get; set; }
        [Comment("Reference to parent group")]
        public Guid? ParentId { get; set; }
        #endregion Foreign keys
    }
}

