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

using System.Collections.Generic;

namespace OpenHolidaysApi.CLI
{
    /// <summary>
    /// Raw data sources
    /// </summary>
    public class SourcesConfiguration
    {
        /// <summary>
        /// File name of countries
        /// </summary>
        public string CountriesFileName { get; set; }

        /// <summary>
        /// File name of languagaes
        /// </summary>
        public string LanguagesFileName { get; set; }
        
        /// <summary>
        /// List of regional data files
        /// </summary>
        public ICollection<RegionalConfiguration> Regional { get; set; }
        
        /// <summary>
        /// Root folder of al the raw data files
        /// </summary>
        public string RootFolderName { get; set; }
    }
}
