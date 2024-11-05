﻿#region OpenHolidays API - Copyright (C) STÜBER SYSTEMS GmbH
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

using Swashbuckle.AspNetCore.Annotations;

namespace OpenHolidaysApi
{
    /// <summary>
    /// Status of a holiday
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public enum HolidayStatus
    {
        /// <summary>
        /// Default
        /// </summary>
        Default = 1,

        /// <summary>
        /// Optional
        /// </summary>
        Optional = 2,

        /// <summary>
        /// Recommended
        /// </summary>
        Recommended = 3
    }
}
