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

using OpenHolidaysApi.DataLayer;

namespace OpenHolidaysApi.CLI
{
    /// <summary>
    /// Database configuration
    /// </summary>
    public class DatabaseConfiguration : IDbConfiguration
    {
        /// <summary>
        /// Database name
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Server port
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// Server name
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string UserName { get; set; }
    }
}
