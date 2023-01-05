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
    /// Representation of a holiday
    /// </summary>
    [Table(DbTables.Holiday)]
    [Comment("Representation of a holiday")]
    public class Holiday : BaseEntity
    {
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
        /// Additional detailed information
        /// </summary>
        [Required]
        [Comment("Additional detailed information")]
        public HolidayDetails Details { get; set; }

        /// <summary>
        /// End date of the holiday
        /// </summary>
        [Required]
        [Comment("End date of the holiday")]
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// Localized names of the holiday
        /// </summary>
        [Required]
        [Column(TypeName = "jsonb")]
        [Comment("Localized names of the holiday")]
        public ICollection<LocalizedText> Names { get; set; } = new List<LocalizedText>();

        /// <summary>
        /// Is this a nationwide holiday?
        /// </summary>
        [Required]
        [Comment("Is this a nationwide holiday?")]
        public bool Nationwide { get; set; }

        /// <summary>
        /// Additional organizational untis
        /// </summary>
        public ICollection<OUnit> OUnits { get; set; } = new List<OUnit>();

        /// <summary>
        /// Start date of the holiday
        /// </summary>
        [Required]
        [Comment("Start date of the holiday")]
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// List of subdivisions 
        /// </summary>
        public virtual ICollection<Subdivision> Subdivisions { get; set; } = new List<Subdivision>();

        /// <summary>
        /// Type of holiday
        /// </summary>
        [Required]
        [Comment("Type of holiday")]
        public HolidayType Type { get; set; }

        #region Foreign keys
        [Comment("Reference to country")]
        public Guid CountryId { get; set; }
        #endregion Foreign keys
    }
}

