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

using Swashbuckle.AspNetCore.Annotations;

namespace OpenHolidayApi
{
    /// <summary>
    /// Type of holiday
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public enum HolidayType
    {
        /// <summary>
        /// No holiday (for information purposes only)
        /// </summary>
        None = 0,

        /// <summary>
        /// Public holiday
        /// </summary>
        Public = 1,

        /// <summary>
        /// Bank holiday
        /// </summary>
        Bank = 2,

        /// <summary>
        /// School holiday
        /// </summary>
        School = 3
    }
}
