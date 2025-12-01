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
    /// CSV record of a holiday 
    /// </summary>
    public class CsvHoliday : CsvBase
    {
        /// <summary>
        /// Additional localized comments
        /// </summary>
        public ICollection<CsvLocalizedText> Comment { get; set; } = [];

        /// <summary>
        /// ISO 3166-1 country code
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// End date
        /// </summary>
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// List of groups
        /// </summary>
        public ICollection<string> Groups { get; set; } = [];

        /// <summary>
        /// Unqiue holiday id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Localized holiday names
        /// </summary>
        public ICollection<CsvLocalizedText> Name { get; set; } = [];

        /// <summary>
        /// Regional scope of a holiday
        /// </summary>
        public RegionalScope RegionalScope { get; set; }

        /// <summary>
        /// Start date
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// List of subdivisions
        /// </summary>
        public ICollection<string> Subdivisions { get; set; } = [];

        /// <summary>
        /// Additional holiday tags
        /// </summary>
        public HolidayTags? Tags { get; set; }

        /// <summary>
        /// Temporal scope of a holiday
        /// </summary>
        public TemporalScope? TemporalScope { get; set; }

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
                RegionalScope = RegionalScope,
                TemporalScope = TemporalScope != null ? (TemporalScope)TemporalScope : DataLayer.TemporalScope.FullDay,
                StartDate = StartDate,
                EndDate = EndDate != DateOnly.MinValue ? EndDate : StartDate,
                Tags = Tags != null ? (HolidayTags)Tags : 0
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
                throw new CsvImportException("No names definied");
            }

            var countryId = await dbContext.Set<Country>().Where(x => x.IsoCode == Country).Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);
            if (countryId != default)
            {
                holiday.CountryId = countryId;
            }
            else
            {
                throw new CsvImportException("Unkown country");
            }

            if (Subdivisions != null && Subdivisions.Count > 0)
            {
                foreach (var csvSubdivison in Subdivisions)
                {
                    var subdivison = await dbContext.Set<Subdivision>()
                        .Where(x => x.CountryId == holiday.CountryId && x.ShortName == csvSubdivison)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (subdivison != null)
                    {
                        holiday.Subdivisions.Add(subdivison);
                    }
                    else
                    {
                        throw new CsvImportException("Unkown subdivision");
                    }
                }
                holiday.HasSubdivisions = true;
            }
            else
            {
                holiday.HasSubdivisions = false;
            }

            if (Groups != null && Groups.Count > 0)
            {
                foreach (var csvGroup in Groups)
                {
                    var group = await dbContext.Set<Group>()
                        .Include(x => x.Subdivisions)
                        .Where(x => x.CountryId == holiday.CountryId && x.ShortName == csvGroup)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (group != null)
                    {
                        holiday.Groups.Add(group);
                        if (holiday.Subdivisions.Count == 0)
                        {
                            foreach (var subdivision in group.Subdivisions)
                            {
                                holiday.Subdivisions.Add(subdivision);
                            }
                        }
                    }
                    else
                    {
                        throw new CsvImportException("Unkown group");
                    }
                }
                holiday.HasGroups = true;
            }
            else
            {
                holiday.HasGroups = false;
            }

            holiday.Nationwide = !holiday.HasSubdivisions && !holiday.HasGroups;

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