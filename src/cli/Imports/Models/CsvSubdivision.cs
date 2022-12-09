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
        /// Additional localized notes
        /// </summary>
        public ICollection<CsvLocalizedText> Comments { get; set; } = new List<CsvLocalizedText>();

        /// <summary>
        /// ISO 3166-1 country code
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// ISO 3166-2 subdivision code
        /// </summary>
        public string IsoCode { get; set; }

        /// <summary>
        /// Localized subdivision names 
        /// </summary>
        public ICollection<CsvLocalizedText> Names { get; set; } = new List<CsvLocalizedText>();

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
                IsoCode = IsoCode,
                ShortName = ShortName
            };

            if (Names != null && Names.Count > 0)
            {
                foreach (var csvName in Names)
                {
                    subdivision.Names.Add(new LocalizedText { Language = csvName.Language, Text = csvName.Text });
                }
            }
            else
            {
                throw new Exception("Error");
            }

            var countryId = await dbContext.Set<Country>().Where(x => x.IsoCode == Country).Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);
            if (countryId != default)
            {
                subdivision.CountryId = countryId;
            }
            else
            {
                throw new Exception("Error");
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
                throw new Exception("Error");
            }

            if (Comments != null && Comments.Count > 0)
            {
                foreach (var csvComment in Comments)
                {
                    subdivision.Comments.Add(new LocalizedText { Language = csvComment.Language, Text = csvComment.Text });
                }
            }

            dbContext.Set<Subdivision>().Add(subdivision);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
