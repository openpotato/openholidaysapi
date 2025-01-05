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
    /// <param name="dbContext">Injected database context</param>
    [Route("/")]
    [SwaggerTag("Reads public and school holidays")]
    public class HolidaysController(AppDbContext dbContext) : BaseController(dbContext)
    {
        /// <summary>
        /// Valid date range for holiday requests
        /// </summary>
        public const int ValidDateRange = 365 * 3; // 3 years

        /// <summary>
        /// Returns list of public holidays for a given country
        /// </summary>
        /// <param name="countryIsoCode" example="DE">ISO 3166-1 code of the country</param>
        /// <param name="validFrom" example="2023-01-01">Start of the date range</param>
        /// <param name="validTo" example="2023-12-31">End of the date range</param>
        /// <param name="languageIsoCode" example="DE">ISO-639-1 code of a language or empty</param>
        /// <param name="subdivisionCode" example="DE-BE">Code of the subdivision or empty</param>
        /// <returns>List of holidays</returns>
        [HttpGet("PublicHolidays")]
        [ProducesResponseType(typeof(IEnumerable<HolidayResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Calendar, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        public async Task<IEnumerable<HolidayResponse>> GetPublicHolidaysAsync(
            [FromQuery, Required] string countryIsoCode, 
            [FromQuery, Required] DateOnly validFrom, 
            [FromQuery, Required] DateOnly validTo,
            [FromQuery] string languageIsoCode = null,
            [FromQuery] string subdivisionCode = null)
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
                            x.Subdivisions.Any(sd =>
                                CodeUtils.BuildStackOfCodes(subdivisionCode).Contains(sd.Code) ||
                                EF.Functions.Like(sd.Code, $"{subdivisionCode}-%")
                            )
                        ) &&
                        (
                            (HolidayType)x.Type == HolidayType.Public ||
                            (HolidayType)x.Type == HolidayType.Bank ||
                            (HolidayType)x.Type == HolidayType.Optional
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
                throw new BadHttpRequestException($"The maximum date range is {ValidDateRange} days.");
            }
        }

        /// <summary>
        /// Returns a list of public holidays from all countries for a given date.
        /// </summary>
        /// <param name="languageIsoCode" example="DE">ISO-639-1 code of a language or empty</param>
        /// <param name="date" example="2023-12-25">Date of interest</param>
        /// <returns>List of holidays</returns>
        [HttpGet("PublicHolidaysByDate")]
        [ProducesResponseType(typeof(IEnumerable<HolidayByDateResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        public async Task<IEnumerable<HolidayByDateResponse>> GetPublicHolidaysByDateAsync(
            [FromQuery, Required] DateOnly date,
            [FromQuery] string languageIsoCode = null)
        {
            return await _dbContext.Set<Holiday>()
                .AsNoTracking()
                .Include(x => x.Country)
                .Include(x => x.Subdivisions)
                .Where(x =>
                    (
                        (HolidayType)x.Type == HolidayType.Public ||
                        (HolidayType)x.Type == HolidayType.Bank ||
                        (HolidayType)x.Type == HolidayType.Optional
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
        /// <param name="validFrom" example="2023-01-01">Start of the date range</param>
        /// <param name="validTo" example="2023-12-31">End of the date range</param>
        /// <param name="languageIsoCode" example="DE">ISO-639-1 code of a language or empty</param>
        /// <param name="subdivisionCode" example="DE-MV">Code of the subdivision or empty</param>
        /// <returns>List of holidays</returns>
        [HttpGet("SchoolHolidays")]
        [ProducesResponseType(typeof(IEnumerable<HolidayResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Calendar, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        public async Task<IEnumerable<HolidayResponse>> GetSchoolHolidaysAsync(
            [FromQuery, Required] string countryIsoCode,
            [FromQuery, Required] DateOnly validFrom,
            [FromQuery, Required] DateOnly validTo,
            [FromQuery] string languageIsoCode = null,
            [FromQuery] string subdivisionCode = null)
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
                            x.Subdivisions.Any(sd =>
                                CodeUtils.BuildStackOfCodes(subdivisionCode).Contains(sd.Code) ||
                                EF.Functions.Like(sd.Code, $"{subdivisionCode}-%")
                            )
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
                throw new BadHttpRequestException($"The maximum date range is {ValidDateRange} days.");
            }
        }

        /// <summary>
        /// Returns a list of school holidays from all countries for a given date.
        /// </summary>
        /// <param name="languageIsoCode" example="DE">ISO-639-1 code of a language or empty</param>
        /// <param name="date" example="2023-12-25">Date of interest</param>
        /// <returns>List of holidays</returns>
        [HttpGet("SchoolHolidaysByDate")]
        [ProducesResponseType(typeof(IEnumerable<HolidayByDateResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        public async Task<IEnumerable<HolidayByDateResponse>> GetSchoolHolidaysByDateAsync(
            [FromQuery, Required] DateOnly date,
            [FromQuery] string languageIsoCode = null)
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
