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

namespace OpenHolidaysApi.CLI
{
    /// <summary>
    /// A localized text
    /// </summary>
    public class CsvLocalizedText
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvLocalizedText"/> class.
        /// </summary>
        /// <param name="language">ISO-639-2 language code</param>
        /// <param name="text">Text content</param>
        public CsvLocalizedText(string language, string text)
        {
            Language = language;
            Text = text;
        }

        /// <summary>
        /// ISO-639-2 language code 
        /// </summary>
        public string Language { get; }

        /// <summary>
        /// Text content
        /// </summary>
        public string Text { get; }
    }
}

