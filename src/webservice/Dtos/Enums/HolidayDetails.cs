﻿#region OpenHolidays API - Copyright (C) 2022 STÜBER SYSTEMS GmbH
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

using Swashbuckle.AspNetCore.Annotations;

namespace OpenHolidayApi
{
    /// <summary>
    /// Detailed information
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public enum HolidayDetails
    {
        /// <summary>
        /// No more details
        /// </summary>
        None = 0,

        /// <summary>
        /// End of lessons
        /// </summary>
        EndOfLessons = 1,

        /// <summary>
        /// Back to school
        /// </summary>
        BackToSchool = 2
    }
}