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
using OpenHolidaysApi.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpenHolidaysApi.CLI
{
    /// <summary>
    /// CSV record of a holiday 
    /// </summary>
    public class CsvHoliday : CsvBase
    {
        /// <summary>
        /// Additional localized comments
        /// </summary>
        public ICollection<CsvLocalizedText> Comment { get; set; } = new List<CsvLocalizedText>();

        /// <summary>
        /// ISO 3166-1 country code
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// End date
        /// </summary>
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// Unqiue holiday id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Localized holiday names
        /// </summary>
        public ICollection<CsvLocalizedText> Name { get; set; } = new List<CsvLocalizedText>();

        /// <summary>
        /// Start date
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// List of subdivisions
        /// </summary>
        public ICollection<string> Subdivisions { get; set; } = new List<string>();

        /// <summary>
        /// Type of holiday
        /// </summary>
        public HolidayType Type { get; set; }

        /// <summary>
        /// Adds this CSV record to the database
        /// </summary>
        /// <param name="dbContext">Database context</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal override async Task AddToDatabase(AppDbContext dbContext, CancellationToken cancellationToken)
        {
            var holiday = new Holiday
            {
                Id = Id,
                Type = Type,
                StartDate = StartDate,
                EndDate = EndDate != DateOnly.MinValue ? EndDate : StartDate
            };

            if (Name != null && Name.Count > 0)
            {
                foreach (var csvName in Name)
                {
                    holiday.Name.Add(new LocalizedText { Language = csvName.Language, Text = csvName.Text });
                }
            }
            else
            {
                throw new Exception("No names definied");
            }

            var countryId = await dbContext.Set<Country>().Where(x => x.IsoCode == Country).Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);
            if (countryId != default)
            {
                holiday.CountryId = countryId;
            }
            else
            {
                throw new Exception("Unkown country");
            }

            if (Subdivisions != null && Subdivisions.Count > 0)
            {
                foreach (var csvSubdivison in Subdivisions)
                {
                    var subdivison = await dbContext.Set<Subdivision>()
                        .Include(x => x.Children)
                        .Where(x => x.CountryId == holiday.CountryId && x.ShortName == csvSubdivison)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (subdivison != null)
                    {
                        if (subdivison.Children.Count > 0)
                        {
                            foreach (var childSubdivison in subdivison.Children)
                            {
                                holiday.Subdivisions.Add(childSubdivison);
                            }
                        }
                        else
                        {
                            holiday.Subdivisions.Add(subdivison);
                        }
                    }
                    else
                    {
                        throw new Exception("Unkown subdivision");
                    }
                }
                holiday.Nationwide = false;
            }
            else
            {
                holiday.Nationwide = true;
            }

            if (Comment != null && Comment.Count > 0)
            {
                foreach (var csvComment in Comment)
                {
                    holiday.Comment.Add(new LocalizedText { Language = csvComment.Language, Text = csvComment.Text });
                }
            }

            dbContext.Set<Holiday>().Add(holiday);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}