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
    /// API controller for regional data
    /// </summary>
    [SwaggerTag("Reads countries, languages, subdivisions and organizational units")]
    public class RegionalController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegionalController"/> class.
        /// </summary>
        /// <param name="dbContext">Injected database context</param>
        public RegionalController(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <summary>
        /// Returns a list of all supported countries
        /// </summary>
        /// <returns>List of countries</returns>
        [HttpGet("Countries")]
        [Produces("text/json", "application/json")]
        public async Task<IEnumerable<CountryResponse>> GetCountriesAsync()
        {
            return await _dbContext.Set<Country>()
                .AsNoTracking()
                .OrderBy(x => x.IsoCode)
                .Select(x => new CountryResponse(x))
                .ToListAsync();
        }

        /// <summary>
        /// Returns a list of all used languages
        /// </summary>
        /// <returns>List of languages</returns>
        [HttpGet("Languages")]
        [Produces("text/json", "application/json")]
        public async Task<IEnumerable<LanguageResponse>> GetLanguagesAsync()
        {
            return await _dbContext.Set<Language>()
                .AsNoTracking()
                .OrderBy(x => x.IsoCode)
                .Select(x => new LanguageResponse(x))
                .ToListAsync();
        }

        /// <summary>
        /// Returns a list of relevant organizational units for a supported country (if any)
        /// </summary>
        /// <param name="countryIsoCode" example="FR">ISO 3166-1 code of the country</param>
        /// <param name="subdivisionIsoCode">ISO 3166-2 code of the subdivision or empty</param>
        /// <returns>List of organizational units</returns>
        [HttpGet("OUnits")]
        [Produces("text/json", "application/json")]
        public async Task<IEnumerable<OUnitResponse>> GetOUnitsAsync([Required] string countryIsoCode, string subdivisionIsoCode)
        {
            return await _dbContext.Set<OUnit>()
                .AsNoTracking()
                .Include(x => x.Parent)
                .Include(x => x.Subdivisions)
                .Where(x =>
                    x.Country.IsoCode == countryIsoCode &&
                    (
                        string.IsNullOrEmpty(subdivisionIsoCode) || x.Subdivisions.Any(sd => sd.IsoCode == subdivisionIsoCode)
                    ))
                .OrderBy(x => x.Code)
                .Select(x => new OUnitResponse(x))
                .ToListAsync();
        }

        /// <summary>
        /// Returns a list of relevant subdivisions for a supported country (if any)
        /// </summary>
        /// <param name="countryIsoCode" example="DE">ISO 3166-1 code of the country</param>
        /// <returns>List of subdivisions</returns>
        [HttpGet("Subdivisions")]
        [Produces("text/json", "application/json")]
        public async Task<IEnumerable<SubdivisionResponse>> GetSubdivisionsAsync([Required] string countryIsoCode)
        {
            return await _dbContext.Set<Subdivision>()
                .AsNoTracking()
                .Include(x => x.Parent)
                .Where(x => x.Country.IsoCode == countryIsoCode)
                .OrderBy(x => x.IsoCode)
                .Select(x => new SubdivisionResponse(x))
                .ToListAsync();
        }
    }
}
