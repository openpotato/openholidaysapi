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

using Enbrea.Konsoli;

namespace OpenHolidaysApi.CLI
{
    /// <summary>
    /// Static factory class for <see cref="ConsoleWriter"/> instances
    /// </summary>
    public static class ConsoleWriterFactory
    {
        /// <summary>
        /// Cretaes a new <see cref="ConsoleWriter"/> instances.
        /// </summary>
        /// <param name="progressValueUnit">The progress value unit</param>
        /// <returns>The new instance</returns>
        public static ConsoleWriter CreateConsoleWriter(ProgressUnit progressValueUnit)
        {
            return new ConsoleWriter(progressValueUnit)
            {
                Theme = new ConsoleWriterTheme()
                {
                    ProgressTextFormat = "> {0}",
                    SuccessLabel = ">"
                }
            };
        }
    }
}
