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

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenHolidaysApi.DataLayer;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace OpenHolidaysApi
{
    /// <summary>
    /// API controller for holiday data
    /// </summary>
    [SwaggerTag("Reads public and school holidays")]
    public class HolidaysController : BaseController
    {
        /// <summary>
        /// Valid date range for holiday requests
        /// </summary>
        public const int ValidDateRange = 365 * 3; // 3 years

        /// <summary>
        /// Initializes a new instance of the <see cref="HolidaysController"/> class.
        /// </summary>
        /// <param name="dbContext">Injected database context</param>
        public HolidaysController(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <summary>
        /// Returns list of public holidays for a given country
        /// </summary>
        /// <param name="countryIsoCode" example="DE">ISO 3166-1 code of the country</param>
        /// <param name="languageIsoCode" example="DE">ISO-639-1 code of a language or empty</param>
        /// <param name="validFrom" example="2023-01-01">Start of the date range</param>
        /// <param name="validTo" example="2023-12-31">End of the date range</param>
        /// <param name="subdivisionCode" example="DE-BE">Code of the subdivision or empty</param>
        /// <returns>List of holidays</returns>
        [HttpGet("PublicHolidays")]
        [Produces("text/plain", "text/json", "application/json", "text/calendar", "text/csv")]
        public async Task<IEnumerable<HolidayResponse>> GetPublicHolidaysAsync([Required] string countryIsoCode, string languageIsoCode, [Required] DateOnly validFrom, [Required] DateOnly validTo, string subdivisionCode)
        {
            if (DateOnlyUtils.DaysBetween(validFrom, validTo) <= ValidDateRange)
            {
                return await _dbContext.Set<Holiday>()
                    .AsNoTracking()
                    .Include(x => x.Country)
                    .Include(x => x.Subdivisions)
                    .Where(x =>
                        x.Country.IsoCode == countryIsoCode &&
                        (
                            string.IsNullOrEmpty(subdivisionCode) ||
                            x.Nationwide ||
                            x.Subdivisions.Any(sd => sd.Code == subdivisionCode || EF.Functions.Like(sd.Code, $"{subdivisionCode}-%"))
                        ) &&
                        (
                            (HolidayType)x.Type == HolidayType.Public ||
                            (HolidayType)x.Type == HolidayType.National ||
                            (HolidayType)x.Type == HolidayType.Regional ||
                            (HolidayType)x.Type == HolidayType.Local ||
                            (HolidayType)x.Type == HolidayType.Bank
                        ) &&
                        (
                            (x.StartDate >= validFrom && x.StartDate <= validTo) ||
                            (x.EndDate >= validFrom && x.EndDate <= validTo) ||
                            (x.StartDate < validFrom && x.EndDate > validTo)
                        ))
                    .OrderBy(x => x.StartDate)
                    .Select(x => new HolidayResponse(x, languageIsoCode))
                    .ToListAsync();
            }
            else
            {
                throw new ArgumentException($"The maximum date range is {ValidDateRange} days.");
            }
        }

        /// <summary>
        /// Returns a list of public holidays from all countries for a given date.
        /// </summary>
        /// <param name="languageIsoCode" example="DE">ISO-639-1 code of a language or empty</param>
        /// <param name="date" example="2023-12-25">Date of interest</param>
        /// <returns>List of holidays</returns>
        [HttpGet("PublicHolidaysByDate")]
        [Produces("text/plain", "text/json", "application/json", "text/csv")]
        public async Task<IEnumerable<HolidayByDateResponse>> GetPublicHolidaysByDateAsync(string languageIsoCode, [Required] DateOnly date)
        {
            return await _dbContext.Set<Holiday>()
                .AsNoTracking()
                .Include(x => x.Country)
                .Include(x => x.Subdivisions)
                .Where(x =>
                    (
                        (HolidayType)x.Type == HolidayType.Public ||
                        (HolidayType)x.Type == HolidayType.National ||
                        (HolidayType)x.Type == HolidayType.Regional ||
                        (HolidayType)x.Type == HolidayType.Local ||
                        (HolidayType)x.Type == HolidayType.Bank
                    ) &&
                    (
                        (x.StartDate <= date && x.EndDate >= date)
                    ))
                .OrderBy(x => x.StartDate)
                .Select(x => new HolidayByDateResponse(x, languageIsoCode))
                .ToListAsync();
        }

        /// <summary>
        /// Returns list of official school holidays for a given country 
        /// </summary>
        /// <param name="countryIsoCode" example="DE">ISO 3166-1 code of the country</param>
        /// <param name="languageIsoCode" example="DE">ISO-639-1 code of a language or empty</param>
        /// <param name="validFrom" example="2023-01-01">Start of the date range</param>
        /// <param name="validTo" example="2023-12-31">End of the date range</param>
        /// <param name="subdivisionCode" example="DE-MV">Code of the subdivision or empty</param>
        /// <returns>List of holidays</returns>
        [HttpGet("SchoolHolidays")]
        [Produces("text/plain", "text/json", "application/json", "text/calendar", "text/csv")]
        public async Task<IEnumerable<HolidayResponse>> GetSchoolHolidaysAsync([Required] string countryIsoCode, string languageIsoCode, [Required] DateOnly validFrom, [Required] DateOnly validTo, string subdivisionCode)
        {
            if (DateOnlyUtils.DaysBetween(validFrom, validTo) <= ValidDateRange)
            {
                return await _dbContext.Set<Holiday>()
                    .AsNoTracking()
                    .Include(x => x.Country)
                    .Include(x => x.Subdivisions)
                    .Where(x =>
                        x.Country.IsoCode == countryIsoCode &&
                        (
                            string.IsNullOrEmpty(subdivisionCode) ||
                            x.Nationwide ||
                            x.Subdivisions.Any(sd => sd.Code == subdivisionCode || EF.Functions.Like(sd.Code, $"{subdivisionCode}-%"))
                        ) &&
                        (
                            (HolidayType)x.Type == HolidayType.School ||
                            (HolidayType)x.Type == HolidayType.BackToSchool ||
                            (HolidayType)x.Type == HolidayType.EndOfLessons
                        ) &&
                        (
                            (x.StartDate >= validFrom && x.StartDate <= validTo) ||
                            (x.EndDate >= validFrom && x.EndDate <= validTo) ||
                            (x.StartDate < validFrom && x.EndDate > validTo)
                        ))
                    .OrderBy(x => x.StartDate)
                    .Select(x => new HolidayResponse(x, languageIsoCode))
                    .ToListAsync();
            }
            else
            {
                throw new ArgumentException($"The maximum date range is {ValidDateRange} days.");
            }
        }

        /// <summary>
        /// Returns a list of school holidays from all countries for a given date.
        /// </summary>
        /// <param name="languageIsoCode" example="DE">ISO-639-1 code of a language or empty</param>
        /// <param name="date" example="2023-12-25">Date of interest</param>
        /// <returns>List of holidays</returns>
        [HttpGet("SchoolHolidaysByDate")]
        [Produces("text/plain", "text/json", "application/json", "text/csv")]
        public async Task<IEnumerable<HolidayByDateResponse>> GetSchoolHolidaysByDateAsync(string languageIsoCode, [Required] DateOnly date)
        {
            return await _dbContext.Set<Holiday>()
                .AsNoTracking()
                .Include(x => x.Country)
                .Include(x => x.Subdivisions)
                .Where(x =>
                    (
                        (HolidayType)x.Type == HolidayType.School || 
                        (HolidayType)x.Type == HolidayType.BackToSchool || 
                        (HolidayType)x.Type == HolidayType.EndOfLessons
                    ) &&
                    (
                        (x.StartDate <= date && x.EndDate >= date)
                    ))
                .OrderBy(x => x.StartDate)
                .Select(x => new HolidayByDateResponse(x, languageIsoCode))
                .ToListAsync();
        }
    }
}
