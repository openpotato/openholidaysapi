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
using OpenHolidaysApi.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpenHolidaysApi.CLI
{
    /// <summary>
    /// A CSV group record  (e.g. a holiday zone)
    /// </summary>
    public class CsvGroup : CsvBase
    {
        /// <summary>
        /// Localized zone categories
        /// </summary>
        public ICollection<CsvLocalizedText> Category { get; set; } = [];

        /// <summary>
        /// Zone code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Additional localized notes
        /// </summary>
        public ICollection<CsvLocalizedText> Comment { get; set; } = [];

        /// <summary>
        /// ISO 3166-1 country code
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Localized subdivision names 
        /// </summary>
        public ICollection<CsvLocalizedText> Name { get; set; } = [];

        /// <summary>
        /// Code of the parent zone
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        /// Short name of the zone
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Adds this CSV record to the database
        /// </summary>
        /// <param name="dbContext">Database context</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal override async Task AddToDatabase(AppDbContext dbContext, CancellationToken cancellationToken)
        {
            var group = new Group
            {
                Code = Code,
                ShortName = ShortName
            };

            if (Name != null && Name.Count > 0)
            {
                foreach (var csvName in Name)
                {
                    group.Name.Add(new LocalizedText { Language = csvName.Language, Text = csvName.Text });
                }
            }
            else
            {
                throw new CsvImportException("No group names definied");
            }

            var countryId = await dbContext.Set<Country>().Where(x => x.IsoCode == Country).Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);
            if (countryId != default)
            {
                group.CountryId = countryId;
            }
            else
            {
                throw new CsvImportException("Unkown country");
            }

            if (Category != null && Category.Count > 0)
            {
                foreach (var csvCategory in Category)
                {
                    group.Category.Add(new LocalizedText { Language = csvCategory.Language, Text = csvCategory.Text });
                }
            }
            else
            {
                throw new CsvImportException("No official country names definied");
            }

            if (!string.IsNullOrEmpty(Parent))
            {
                var parentId = await dbContext.Set<Group>()
                    .Where(x => x.CountryId == group.CountryId && x.ShortName == Parent)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (parentId != default)
                {
                    group.ParentId = parentId;
                }
                else
                {
                    throw new CsvImportException("Parent not known");
                }
            }

            if (Comment != null && Comment.Count > 0)
            {
                foreach (var csvComment in Comment)
                {
                    group.Comment.Add(new LocalizedText { Language = csvComment.Language, Text = csvComment.Text });
                }
            }

            dbContext.Set<Group>().Add(group);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
