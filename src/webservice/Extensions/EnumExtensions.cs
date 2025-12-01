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

namespace OpenHolidaysApi
{
    /// <summary>
    /// Extension methods for <see cref="Enum"/>
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Converts a flagged <typeparamref name="T"/> enum value into an array of strings.
        /// </summary>
        /// <typeparam name="T">An enum type with the <see cref="Enum"/> constraint.</typeparam>
        /// <param name="enumValue">The enum instance whose active flag names should be extracted.</param>
        /// <returns>
        /// An array of strings containing the names of each active flag.
        /// </returns>
        public static string[] ToStringArray<T>(this T enumValue) where T : struct, Enum
        {
            return Enum.GetValues<T>()
                .Where(v => Convert.ToInt64(v) != 0 && enumValue.HasFlag(v))
                .Select(v => v.ToString())
                .ToArray();
        }

        /// <summary>
        /// Converts a nullable flagged <typeparamref name="T"/> enum value into a string array.
        /// </summary>
        /// <typeparam name="T">An enum type with the <see cref="Enum"/> constraint.</typeparam>
        /// <param name="enumValue">Nullable enum value to evaluate.</param>
        /// <returns>
        /// An array of strings containing the names of each active flag, or an empty array if <paramref name="enumValue"/> is <c>null</c>.
        /// </returns>
        public static string[] ToStringArray<T>(this T? enumValue) where T : struct, Enum
        {
            if (enumValue.HasValue)
            {
                return enumValue.Value.ToStringArray();
            }
            else
            {
                return [];
            }
        }
    }
}
