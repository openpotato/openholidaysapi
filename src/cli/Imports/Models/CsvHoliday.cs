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
        public ICollection<CsvLocalizedText> Comments { get; set; } = new List<CsvLocalizedText>();

        /// <summary>
        /// ISO 3166-1 country code
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Additional detail information
        /// </summary>
        public HolidayDetails? Details { get; set; }

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
        public ICollection<CsvLocalizedText> Names { get; set; } = new List<CsvLocalizedText>();
        /// <summary>
        /// List of organizational units
        /// </summary>
        public ICollection<string> OUnits { get; set; } = new List<string>();

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
                Details = Details != null ? (HolidayDetails)Details : HolidayDetails.None,
                StartDate = StartDate,
                EndDate = EndDate != DateOnly.MinValue ? EndDate : StartDate
            };

            if (Names != null && Names.Count > 0)
            {
                foreach (var csvName in Names)
                {
                    holiday.Names.Add(new LocalizedText { Language = csvName.Language, Text = csvName.Text });
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
                    var subdivison = await dbContext.Set<Subdivision>().Where(x => x.CountryId == holiday.CountryId && x.ShortName == csvSubdivison).FirstOrDefaultAsync(cancellationToken);
                    if (subdivison != null)
                    {
                        holiday.Subdivisions.Add(subdivison);
                    }
                }
                holiday.Nationwide = false;
            }
            else
            {
                foreach (var subdivison in dbContext.Set<Subdivision>().Where(x => x.CountryId == holiday.CountryId))
                {
                    holiday.Subdivisions.Add(subdivison);
                }
                holiday.Nationwide = true;
            }

            if (OUnits != null && OUnits.Count > 0)
            {
                foreach (var csvOUnit in OUnits)
                {
                    var oUnit = await dbContext.Set<OUnit>().Where(x => x.CountryId == holiday.CountryId && x.ShortName == csvOUnit).FirstOrDefaultAsync(cancellationToken);
                    if (oUnit != null)
                    {
                        holiday.OUnits.Add(oUnit);
                    }
                }
                holiday.Nationwide = false;
            }

            if (Comments != null && Comments.Count > 0)
            {
                foreach (var csvComment in Comments)
                {
                    holiday.Comments.Add(new LocalizedText { Language = csvComment.Language, Text = csvComment.Text });
                }
            }

            dbContext.Set<Holiday>().Add(holiday);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}