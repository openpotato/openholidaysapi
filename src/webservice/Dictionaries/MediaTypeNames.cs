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
    /// Used media types for the HTTP Content-Type representation header
    /// </summary>
    /// <remarks>
    /// See: https://www.iana.org/assignments/media-types/media-types.xhtml
    /// </remarks>
    public static class MediaTypeNames
    {
        /// <summary>
        /// Application Media Types
        /// </summary>
        public static class Application
        {
            /// <summary>
            /// Json
            /// </summary>
            public const string Json = "application/json";

            /// <summary>
            /// Problem Details
            /// </summary>
            public const string ProblemDetails = "application/problem+json";
        }

        /// <summary>
        /// Text Media Types
        /// </summary>
        public static class Text
        {
            /// <summary>
            /// Calendar
            /// </summary>
            public const string Calendar = "text/calendar";

            /// <summary>
            /// Csv
            /// </summary>
            public const string Csv = "text/csv";

            /// <summary>
            /// Json
            /// </summary>
            public const string Json = "text/json";

            /// <summary>
            /// Plain
            /// </summary>
            public const string Plain = "text/plain";
        }
    }
}
