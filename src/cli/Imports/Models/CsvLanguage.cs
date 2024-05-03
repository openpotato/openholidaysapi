﻿#region OpenHolidays API - Copyright (C) STÜBER SYSTEMS GmbH
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
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OpenHolidaysApi.CLI
{
    /// <summary>
    /// CSV record of a language as defined in ISO-639-1
    /// </summary>
    public class CsvLanguage : CsvBase
    {
        /// <summary>
        /// ISO-639-2 language code 
        /// </summary>
        public string IsoCode { get; set; }

        /// <summary>
        /// Localized language names 
        /// </summary>
        public ICollection<CsvLocalizedText> Name { get; set; } = new List<CsvLocalizedText>();

        /// <summary>
        /// Adds this CSV record to the database
        /// </summary>
        /// <param name="dbContext">Database context</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal override async Task AddToDatabase(AppDbContext dbContext, CancellationToken cancellationToken)
        {
            var language = new Language
            {
                IsoCode = IsoCode
            };

            if (Name != null && Name.Count > 0)
            {
                foreach (var csvName in Name)
                {
                    language.Name.Add(new LocalizedText { Language = csvName.Language, Text = csvName.Text });
                }
            }
            else
            {
                throw new Exception("No language names definied");
            }

            dbContext.Set<Language>().Add(language);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
