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

namespace OpenHolidaysApi.CLI
{
    /// <summary>
    /// Regional raw data sources for a specific country
    /// </summary>
    public class RegionalConfiguration
    {
        /// <summary>
        /// Country code
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Folder name of the country
        /// </summary>
        public string CountryFolderName { get; set; }

        /// <summary>
        /// Folder name of holiday files
        /// </summary>
        public string HolidaysFolderName { get; set; }

        /// <summary>
        /// File name of organizational units
        /// </summary>
        public string OUnitsFileName { get; set; }

        /// <summary>
        /// File name of subdivisions
        /// </summary>
        public string SubdivisionsFileName { get; set; }
    }
}
