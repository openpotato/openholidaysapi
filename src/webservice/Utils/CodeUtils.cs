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

using System.Text;

namespace OpenHolidaysApi
{
    /// <summary>
    /// Helper routines for for subdivision codes
    /// </summary>
    public static class CodeUtils
    {
        /// <summary>
        /// Generates a string array which includes a given subdivision code and
        /// all of its parent codes.
        /// </summary>
        /// <param name="subdivisionCode">The subdivision code</param>
        /// <returns>String array of codes</returns>
        public static string[] BuildStackOfCodes(string subdivisionCode)
        {
            var sb = new StringBuilder();
            var codeFragments = subdivisionCode.Split('-');
            if (codeFragments.Length > 1)
            {
                var codeArray = new string[codeFragments.Length - 1];
                for (var i = 0; i < codeFragments.Length - 1; i++)
                {
                    sb.Clear();
                    for (var j = 0; j < i + 2; j++)
                    {
                        if (j > 0) sb.Append('-');
                        sb.Append(codeFragments[j]);
                    }
                    codeArray[i] = sb.ToString();
                }
                return codeArray;
            }
            else
            {
                return codeFragments;
            }
        }
    }
}
