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
    /// Representation of an organizational unit (e.g. a holiday zone)
    /// </summary>
    [Table(DbTables.OUnit)]
    [Index(nameof(Code), IsUnique = true)]
    [Index(nameof(CountryId), nameof(ShortName), IsUnique = true)]
    [Comment("Representation of an organizational unit (e.g. a holiday zone or a school type)")]
    public class OUnit : BaseEntity
    {
        /// <summary>
        /// List of organizational unit children
        /// </summary>
        public virtual ICollection<OUnit> Children { get; set; }

        /// <summary>
        /// Organizational unit code
        /// </summary>
        [Required]
        [Comment("Organizational unit code")]
        public string Code { get; set; }

        /// <summary>
        /// Additional localized comments
        /// </summary>
        [Column(TypeName = "jsonb")]
        [Comment("Additional localized comments")]
        public ICollection<LocalizedText> Comments { get; set; } = new List<LocalizedText>();

        /// <summary>
        /// Reference to country
        /// </summary>
        [Required]
        public Country Country { get; set; }

        /// <summary>
        /// List of holidays (Feiertage oder Ferientage)
        /// </summary>
        public virtual ICollection<Holiday> Holidays { get; set; }

        /// <summary>
        /// Localized organizational unit names 
        /// </summary>
        [Required]
        [Column(TypeName = "jsonb")]
        [Comment("Localized organizational unit names")]
        public ICollection<LocalizedText> Names { get; set; } = new List<LocalizedText>();

        /// <summary>
        /// Reference to parent organizational unit
        /// </summary>
        public OUnit Parent { get; set; }

        /// <summary>
        /// Short name for display
        /// </summary>
        [Required]
        [Comment("Short name for display")]
        public string ShortName { get; set; }

        /// <summary>
        /// References to subdivisions
        /// </summary>
        public virtual ICollection<Subdivision> Subdivisions { get; set; } = new List<Subdivision>();

        #region Foreign keys
        [Comment("Reference to country")]
        public Guid CountryId { get; set; }
        [Comment("Reference to parent organizational unit")]
        public Guid? ParentId { get; set; }
        #endregion Foreign keys
    }
}

