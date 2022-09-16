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

namespace OpenHolidaysApi
{
    /// <summary>
    /// Helper routines for for <see cref="DateOnly"/>
    /// </summary>
    public static class DateOnlyUtils
    {
        /// <summary>
        /// Calculate the number of days between 2 <see cref="DateOnly"/> instances.
        /// </summary>
        /// <param name="firstvalue">First date</param>
        /// <param name="secondValue">Second date</param>
        /// <returns>Number of days</returns>
        public static double DaysBetween(DateOnly firstvalue, DateOnly secondValue)
        {
            return (secondValue.ToDateTime(new TimeOnly(0)) - firstvalue.ToDateTime(new TimeOnly(0))).TotalDays;
        }
    }
}
