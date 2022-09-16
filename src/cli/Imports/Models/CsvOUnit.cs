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
    /// A CSV organizational unit record
    /// </summary>
    public class CsvOUnit : CsvBase
    {
        /// <summary>
        /// Organizational unit code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Additional localized notes
        /// </summary>
        public ICollection<CsvLocalizedText> Comments { get; set; } = new List<CsvLocalizedText>();

        /// <summary>
        /// ISO 3166-1 country code
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Localized organizational unit names 
        /// </summary>
        public ICollection<CsvLocalizedText> Names { get; set; } = new List<CsvLocalizedText>();

        /// <summary>
        /// Code of the parent organizational unit 
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        /// Short name of the subdivision 
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// List of subdivisions
        /// </summary>
        public ICollection<string> Subdivisions { get; set; } = new List<string>();

        /// <summary>
        /// Adds this CSV record to the database
        /// </summary>
        /// <param name="dbContext">Database context</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal override async Task AddToDatabase(OpenHolidaysApiDbContext dbContext, CancellationToken cancellationToken)
        {
            var oUnit = new OUnit
            {
                Code = Code,
                ShortName = ShortName,
            };

            if (Names != null && Names.Count > 0)
            {
                foreach (var csvName in Names)
                {
                    oUnit.Names.Add(new LocalizedText { Language = csvName.Language, Text = csvName.Text });
                }
            }
            else
            {
                throw new Exception("Error");
            }

            var countryId = await dbContext.Set<Country>().Where(x => x.IsoCode == Country).Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);
            if (countryId != default)
            {
                oUnit.CountryId = countryId;
            }
            else
            {
                throw new Exception("Error");
            }

            if (Subdivisions != null && Subdivisions.Count > 0)
            {
                foreach (var csvSubdivison in Subdivisions)
                {
                    var subdivison = await dbContext.Set<Subdivision>().Where(x => x.CountryId == oUnit.CountryId && x.ShortName == csvSubdivison).FirstOrDefaultAsync(cancellationToken);
                    if (subdivison != null)
                    {
                        oUnit.Subdivisions.Add(subdivison);
                    }
                }
            }

            var parentId = await dbContext.Set<OUnit>().Where(x => x.ShortName == Parent).Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);
            if (parentId != default)
            {
                oUnit.ParentId = parentId;
            }

            if (Comments != null && Comments.Count > 0)
            {
                foreach (var csvComment in Comments)
                {
                    oUnit.Comments.Add(new LocalizedText { Language = csvComment.Language, Text = csvComment.Text });
                }
            }

            dbContext.Set<OUnit>().Add(oUnit);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
