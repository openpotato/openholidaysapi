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

using OpenHolidaysApi.DataLayer;

namespace OpenHolidaysApi
{
    /// <summary>
    /// Extension methods for <see cref="ICollection{T}"/>
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Tries to reduce a list of localized text instances to a given language or (if not found) to the default language or (if not found) 
        /// to the first language in the list.
        /// </summary>
        /// <param name="localizedTextList">List of localized text instances</param>
        /// <param name="languageCode">ISO-639-1 language code </param>
        /// <param name="defaultLanguageCode">ISO-639-1 language code </param>
        /// <returns>A list of localized text instances</returns>
        public static List<LocalizedText> ToLocalizedList(this ICollection<DataLayer.LocalizedText> localizedTextList, string languageCode, string defaultLanguageCode = "EN")
        {
            if (string.IsNullOrEmpty(languageCode))
            {
                // Without language code returns ALL localized text entires
                return localizedTextList.Select(x => new LocalizedText { Language = x.Language, Text = x.Text }).ToList();
            }
            else
            {
                // With language code returns either the matching language, the default language or the first language in the array
                if (localizedTextList.Any(x => x.Language == languageCode))
                {
                    return localizedTextList.Where(x => x.Language == languageCode).Select(x => new LocalizedText { Language = x.Language, Text = x.Text }).ToList();
                }
                else if (localizedTextList.Any(x => x.Language == defaultLanguageCode))
                {
                    return localizedTextList.Where(x => x.Language == defaultLanguageCode).Select(x => new LocalizedText { Language = x.Language, Text = x.Text }).ToList();
                }
                else
                {
                    return localizedTextList.Select(x => new LocalizedText { Language = x.Language, Text = x.Text }).Take(1).ToList();
                }
            }
        }

        /// <summary>
        /// Creates a list of <see cref="SubdivisionResponse"/> instances from a list of <see cref="Subdivision"/> instances.
        /// </summary>
        /// <param name="subdivisionList">List of subdivisions</param>
        /// <param name="languageCode">ISO-639-1 language code </param>
        /// <returns>List of <see cref="SubdivisionResponse"/> instances</returns>
        public static List<SubdivisionResponse> ToResponseList(this ICollection<Subdivision> subdivisionList, string languageCode)
        {
            return subdivisionList.Select(x => new SubdivisionResponse(x, languageCode)).ToList();
        }

        /// <summary>
        /// Reduces a list of localized text instances to a given language or (if not found) to the default language or (if not found) 
        /// to the first language in the list and gives back the language value
        /// </summary>
        /// <param name="localizedTextList">List of localized text instances</param>
        /// <param name="languageCode">ISO-639-1 language code </param>
        /// <param name="defaultLanguageCode">ISO-639-1 language code </param>
        /// <returns>A language value</returns>
        public static string ToSingleLanguage(this ICollection<LocalizedText> localizedTextList, string languageCode, string defaultLanguageCode = "EN")
        {
            // Returns either the matching language, the default language or the first language in the array
            if (!string.IsNullOrEmpty(languageCode) && localizedTextList.Any(x => x.Language == languageCode))
            {
                return localizedTextList.Where(x => x.Language == languageCode).First().Language;
            }
            else if (localizedTextList.Any(x => x.Language == defaultLanguageCode))
            {
                return localizedTextList.Where(x => x.Language == defaultLanguageCode).First().Language;
            }
            else
            {
                return localizedTextList.FirstOrDefault()?.Language;
            }
        }

        /// <summary>
        /// Reduces a list of localized text instances to a given language or (if not found) to the default language or (if not found) 
        /// to the first language in the list and gives back the localized text value
        /// </summary>
        /// <param name="localizedTextList">List of localized text instances</param>
        /// <param name="languageCode">ISO-639-1 language code </param>
        /// <param name="defaultLanguageCode">ISO-639-1 language code </param>
        /// <returns>A text value</returns>
        public static string ToSingleText(this ICollection<LocalizedText> localizedTextList, string languageCode, string defaultLanguageCode = "EN")
        {
            // Returns either the matching language, the default language or the first language in the array
            if (!string.IsNullOrEmpty(languageCode) && localizedTextList.Any(x => x.Language == languageCode))
            {
                return localizedTextList.Where(x => x.Language == languageCode).First().Text;
            }
            else if (localizedTextList.Any(x => x.Language == defaultLanguageCode))
            {
                return localizedTextList.Where(x => x.Language == defaultLanguageCode).First().Text;
            }
            else
            {
                return localizedTextList.FirstOrDefault()?.Text;
            }
        }
    }
}
