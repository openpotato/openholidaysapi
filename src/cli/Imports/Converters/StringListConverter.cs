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

using Enbrea.Csv;
using System.Collections.Generic;

namespace OpenHolidaysApi.CLI
{
    /// <summary>
    /// Implementation of a <see cref="ICollection<string>"> converter to or from CSV
    /// </summary>
    public class StringListConverter : ICsvConverter
    {
        public virtual object FromString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }
            else
            {
                var result = new List<string>();
                foreach (var valuePart in value.Split(','))
                {
                    result.Add(valuePart);
                }
                return result;
            }
        }

        public string ToString(object value)
        {
            if (value is ICollection<string> stringList)
            {
                var result = string.Empty;
                foreach (var stringPart in stringList)
                {
                    result = string.Join(',', result, stringPart);
                }
                return result;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}