
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
    /// A CSV subdivision record  (e.g. a federal state)
    /// </summary>
    public class CsvSubdivision : CsvBase
    {
        /// <summary>
        /// Localized subdivision categories
        /// </summary>
        public ICollection<CsvLocalizedText> Category { get; set; } = new List<CsvLocalizedText>();

        /// <summary>
        /// Subdivision code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Additional localized notes
        /// </summary>
        public ICollection<CsvLocalizedText> Comment { get; set; } = new List<CsvLocalizedText>();

        /// <summary>
        /// ISO 3166-1 country code
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// ISO 3166-2 subdivision code (if available)
        /// </summary>
        public string IsoCode { get; set; }

        /// <summary>
        /// Localized subdivision names 
        /// </summary>
        public ICollection<CsvLocalizedText> Name { get; set; } = new List<CsvLocalizedText>();

        /// <summary>
        /// Official languages as ISO-639-1 codes
        /// </summary>
        public ICollection<string> OfficialLanguages { get; set; } = new List<string>();

        /// <summary>
        /// Code of the parent subdivision 
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        /// Short name of the subdivision 
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
            var subdivision = new Subdivision
            {
                Code = Code,
                IsoCode = IsoCode,
                ShortName = ShortName
            };

            if (Name != null && Name.Count > 0)
            {
                foreach (var csvName in Name)
                {
                    subdivision.Name.Add(new LocalizedText { Language = csvName.Language, Text = csvName.Text });
                }
            }
            else
            {
                throw new Exception("No subdivision names definied");
            }

            var countryId = await dbContext.Set<Country>().Where(x => x.IsoCode == Country).Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);
            if (countryId != default)
            {
                subdivision.CountryId = countryId;
            }
            else
            {
                throw new Exception("Unkown country");
            }

            if (Category != null && Category.Count > 0)
            {
                foreach (var csvCategory in Category)
                {
                    subdivision.Category.Add(new LocalizedText { Language = csvCategory.Language, Text = csvCategory.Text });
                }
            }
            else
            {
                throw new Exception("No official country names definied");
            }

            if (OfficialLanguages != null && OfficialLanguages.Count > 0)
            {
                foreach (var languageCode in OfficialLanguages)
                {
                    subdivision.OfficialLanguages.Add(languageCode);
                }
            }
            else
            {
                throw new Exception("No official languages definied");
            }

            if (!string.IsNullOrEmpty(Parent))
            {
                var parentId = await dbContext.Set<Subdivision>()
                    .Where(x => x.CountryId == subdivision.CountryId && x.ShortName == Parent)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (parentId != default)
                {
                    subdivision.ParentId = parentId;
                }
                else
                {
                    throw new Exception("Parent not known");
                }
            }

            if (Comment != null && Comment.Count > 0)
            {
                foreach (var csvComment in Comment)
                {
                    subdivision.Comment.Add(new LocalizedText { Language = csvComment.Language, Text = csvComment.Text });
                }
            }

            dbContext.Set<Subdivision>().Add(subdivision);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
