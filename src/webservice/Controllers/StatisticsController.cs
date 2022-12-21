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

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenHolidaysApi.DataLayer;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace OpenHolidaysApi
{
    /// <summary>
    /// API controller for statistical data 
    /// </summary>
    [Route("Statistics")]
    [SwaggerTag("Reads statistical data about stored holidays")]
    public class StatisticsController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsController"/> class.
        /// </summary>
        /// <param name="dbContext">Injected database context</param>
        public StatisticsController(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <summary>
        /// Returns statistical data about school holidays for a given country
        /// </summary>
        /// <param name="countryIsoCode" example="DE">ISO 3166-1 code of the country</param>
        /// <param name="subdivisionIsoCode" example="DE-BE">ISO 3166-2 code of the subdivision or empty</param>
        /// <returns>Statistical data</returns>
        [HttpGet("SchoolHolidays")]
        [Produces("text/json", "application/json")]
        public async Task<StatisticsResponse> GetSchoolHolidaysAsync([Required] string countryIsoCode, string subdivisionIsoCode)
        {
            DateOnly youngestDate;
            DateOnly oldestDate;

            oldestDate = await _dbContext.Set<Holiday>()
                .AsNoTracking()
                .Where(x =>
                    x.Country.IsoCode == countryIsoCode &&
                    (
                        string.IsNullOrEmpty(subdivisionIsoCode) || x.Subdivisions.Any(sd => sd.IsoCode == subdivisionIsoCode)
                    ) &&
                    (
                        x.Type == HolidayType.School || (x.Type == HolidayType.None && x.Details != HolidayDetails.None)
                    ))
                .OrderBy(x => x.StartDate)
                .Select(x => x.StartDate)
                .FirstOrDefaultAsync();

            youngestDate = await _dbContext.Set<Holiday>()
                .AsNoTracking()
                .Where(x =>
                    x.Country.IsoCode == countryIsoCode &&
                    (
                        string.IsNullOrEmpty(subdivisionIsoCode) || x.Subdivisions.Any(sd => sd.IsoCode == subdivisionIsoCode)
                    ) &&
                    (
                        x.Type == HolidayType.School || (x.Type == HolidayType.None && x.Details != HolidayDetails.None)
                    ))
                .OrderByDescending(x => x.StartDate)
                .Select(x => x.StartDate)
                .FirstOrDefaultAsync();

            return new StatisticsResponse(youngestDate, oldestDate);
        }

        /// <summary>
        /// Returns statistical data about public holidays for a given country.
        /// </summary>
        /// <param name="countryIsoCode" example="DE">ISO 3166-1 code of the country</param>
        /// <param name="subdivisionIsoCode" example="DE-BE">ISO 3166-2 code of the subdivision or empty</param>
        /// <returns>Statistical data</returns>
        [HttpGet("PublicHolidays")]
        [Produces("text/json", "application/json")]
        public async Task<StatisticsResponse> GetPublicHolidaysAsync([Required] string countryIsoCode, string subdivisionIsoCode)
        {
            DateOnly youngestDate;
            DateOnly oldestDate;

            oldestDate = await _dbContext.Set<Holiday>()
                .AsNoTracking()
                .Where(x =>
                    x.Country.IsoCode == countryIsoCode &&
                    (
                        string.IsNullOrEmpty(subdivisionIsoCode) || x.Subdivisions.Any(sd => sd.IsoCode == subdivisionIsoCode)
                    ) &&
                    x.Type == HolidayType.Public)
                .OrderBy(x => x.StartDate)
                .Select(x => x.StartDate)
                .FirstOrDefaultAsync();

            youngestDate = await _dbContext.Set<Holiday>()
                .AsNoTracking()
                .Where(x =>
                    x.Country.IsoCode == countryIsoCode &&
                    (
                        string.IsNullOrEmpty(subdivisionIsoCode) || x.Subdivisions.Any(sd => sd.IsoCode == subdivisionIsoCode)
                    ) &&
                    x.Type == HolidayType.Public)
                .OrderByDescending(x => x.StartDate)
                .Select(x => x.StartDate)
                .FirstOrDefaultAsync();

            return new StatisticsResponse(youngestDate, oldestDate);
        }
    }
}
