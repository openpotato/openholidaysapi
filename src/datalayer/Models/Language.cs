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
    /// Representation of a language
    /// </summary>
    [Table(DbTables.Language)]
    [Index(nameof(IsoCode), IsUnique = true)]
    [Comment("Representation of a language")]
    public class Language : BaseEntity
    {
        /// <summary>
        /// ISO-639-1 language code
        /// </summary>
        [Required]
        [Comment("ISO-639-1 language code")]
        public string IsoCode { get; set; }

        /// <summary>
        /// Language name
        /// </summary>
        [Required]
        [Column(TypeName = "jsonb")]
        [Comment("Localized language names")]
        public ICollection<LocalizedText> Names { get; set; } = new List<LocalizedText>();
    }
}

