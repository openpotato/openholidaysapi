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

using System;

namespace OpenHolidaysApi.DataLayer
{
    /// <summary>
    /// Additional holiday tags
    /// </summary>
    [Flags]
    public enum HolidayTags
    {
        /// <summary>
        /// A recommended holiday date
        /// </summary>
        Recommended = 1,

        /// <summary>
        /// A provisional holiday date
        /// </summary>
        Provisional = 2,

        /// <summary>
        /// An one-time holiday date
        /// </summary>
        OneTime = 4,

        /// <summary>
        /// A holiday date which is an exception from the rule
        /// </summary>
        Exception = 8
    }
}
