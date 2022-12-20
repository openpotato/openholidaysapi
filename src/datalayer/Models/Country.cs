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

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenHolidaysApi.DataLayer
{
    /// <summary>
    /// Representation of a country
    /// </summary>
    [Table(DbTables.Country)]
    [Index(nameof(IsoCode), IsUnique = true)]
    [Comment("Representation of a country")]
    public class Country : BaseEntity
    {
        /// <summary>
        /// ISO 3166-1 country code
        /// </summary>
        [Required]
        [Comment("ISO 3166-1 country code")]
        public string IsoCode { get; set; }

        /// <summary>
        /// Localized country names 
        /// </summary>
        [Required]
        [Column(TypeName = "jsonb")]
        [Comment("Localized country names")]
        public ICollection<LocalizedText> Names { get; set; } = new List<LocalizedText>();

        /// <summary>
        /// ISO-639-1 language codes
        /// </summary>
        [Required]
        [Column(TypeName = "jsonb")]
        [Comment("ISO-639-1 language codes")]
        public ICollection<string> OfficialLanguages { get; set; } = new List<string>();

        /// <summary>
        /// ISO 3166-1 official country names
        /// </summary>
        [Required]
        [Column(TypeName = "jsonb")]
        [Comment("ISO 3166-1 official country names")]
        public ICollection<string> OfficialNames { get; set; } = new List<string>();

        /// <summary>
        /// List of relevant subdivisions 
        /// </summary>
        public virtual ICollection<Subdivision> Subdivisions { get; set; }
    }
}

