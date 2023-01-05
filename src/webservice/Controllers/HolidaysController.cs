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
        /// Returns list of official school holidays for a given country 
        /// </summary>
        /// <param name="countryIsoCode" example="DE">ISO 3166-1 code of the country</param>
        /// <param name="validFrom" example="2022-01-01">Start of the date range</param>
        /// <param name="validTo" example="2022-12-31">End of the date range</param>
        /// <param name="subdivisionIsoCode" example="DE-MV">ISO 3166-2 code of the subdivision or empty</param>
        /// <param name="oUnitCode" example="DE-ABS">Code of an organizational unit or empty</param>
        /// <param name="languageIsoCode" example="DE">ISO-639-1 code of a language or empty</param>
        /// <returns>List of holidays</returns>
        [HttpGet("SchoolHolidays")]
        [Produces("text/json", "application/json", "text/calendar")]
        public async Task<IEnumerable<HolidayResponse>> GetSchoolHolidaysAsync([Required] string countryIsoCode, [Required] DateOnly validFrom, [Required] DateOnly validTo, string subdivisionIsoCode, string oUnitCode, string languageIsoCode)
        {
            if (DateOnlyUtils.DaysBetween(validFrom, validTo) <= ValidDateRange)
            {
                return await _dbContext.Set<Holiday>()
                    .AsNoTracking()
                    .Include(x => x.Country)
                    .Include(x => x.Subdivisions)
                    .Include(x => x.OUnits)
                    .Where(x => 
                        x.Country.IsoCode == countryIsoCode &&
                        (
                            string.IsNullOrEmpty(subdivisionIsoCode) || x.Subdivisions.Any(sd => sd.IsoCode == subdivisionIsoCode)
                        ) &&
                        (
                            string.IsNullOrEmpty(oUnitCode) || x.OUnits.Any(ou => ou.Code == oUnitCode || ou.Children.Any(c => c.Code == oUnitCode))
                        ) &&
                        (
                            x.Type == HolidayType.School || (x.Type == HolidayType.None && x.Details != HolidayDetails.None)
                        ) &&
                        x.StartDate >= validFrom &&
                        x.StartDate <= validTo)
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
        /// Returns list of public holidays for a given country
        /// </summary>
        /// <param name="countryIsoCode" example="DE">ISO 3166-1 code of the country</param>
        /// <param name="validFrom" example="2022-01-01">Start of the date range</param>
        /// <param name="validTo" example="2022-12-31">End of the date range</param>
        /// <param name="subdivisionIsoCode" example="DE-BE">ISO 3166-2 code of the subdivision or empty</param>
        /// <param name="languageIsoCode" example="DE">ISO-639-1 code of a language or empty</param>
        /// <returns>List of holidays</returns>
        [HttpGet("PublicHolidays")]
        [Produces("text/json", "application/json", "text/calendar")]
        public async Task<IEnumerable<HolidayResponse>> GetPublicHolidaysAsync([Required] string countryIsoCode, [Required] DateOnly validFrom, [Required] DateOnly validTo, string subdivisionIsoCode, string languageIsoCode)
        {
            if (DateOnlyUtils.DaysBetween(validFrom, validTo) <= ValidDateRange)
            {
                return await _dbContext.Set<Holiday>()
                    .AsNoTracking()
                    .Include(x => x.Country)
                    .Include(x => x.Subdivisions)
                    .Include(x => x.OUnits)
                    .Where(x =>
                        x.Country.IsoCode == countryIsoCode &&
                        (
                            string.IsNullOrEmpty(subdivisionIsoCode) || x.Subdivisions.Any(sd => sd.IsoCode == subdivisionIsoCode)
                        ) &&
                        (
                            x.Type == HolidayType.Public
                        ) &&
                        x.StartDate >= validFrom &&
                        x.StartDate <= validTo)
                    .OrderBy(x => x.StartDate)
                    .Select(x => new HolidayResponse(x, languageIsoCode))
                    .ToListAsync();
            }
            else
            {
                throw new ArgumentException($"The maximum date range is {ValidDateRange} days.");
            }
        }
    }
}
