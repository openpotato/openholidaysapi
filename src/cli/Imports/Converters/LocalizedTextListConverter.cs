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

using Enbrea.Csv;
using System;
using System.Collections.Generic;

namespace OpenHolidaysApi.CLI
{
    /// <summary>
    /// Implementation of a <see cref="ICollection<CsvLocalizedText>"> converter to or from CSV
    /// </summary>
    public class LocalizedTextListConverter : ICsvConverter
    {
        public virtual object FromString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }
            else 
            {
                var result = new List<CsvLocalizedText>();
                foreach (var localizedText in value.Split(','))
                {
                    result.Add(new CsvLocalizedText(localizedText.Substring(0, 2), Uri.UnescapeDataString(localizedText.Substring(3))));
                }
                return result;
            }
        }

        public string ToString(object value)
        {
            if (value is ICollection<CsvLocalizedText> localizedTextList)
            {
                var result = string.Empty;
                foreach (var localizedText in localizedTextList)
                {
                    result = string.Join(',', result, $"{localizedText.Language} {Uri.EscapeDataString(localizedText.Text)}");
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