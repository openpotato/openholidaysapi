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

namespace OpenHolidaysApi.DataLayer
{
    /// <summary>
    /// Regional scope of a holdiay
    /// </summary>
    public enum RegionalScope
    {
        /// <summary>
        /// National holiday
        /// </summary>
        National = 1,

        /// <summary>
        /// Regional holiday
        /// </summary>
        Regional = 2,

        /// <summary>
        /// Local holiday (usually only optional)
        /// </summary>
        Local = 3,
    }
}
