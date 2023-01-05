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

using System.Text;
using Enbrea.Ics;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace OpenHolidaysApi
{
    /// <summary>
    /// Writes <see cref="HolidayResponse"/> instances formatted as iCalendar to the output stream.
    /// </summary>
    public class IcalOutputFormatter : TextOutputFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IcalOutputFormatter"/> class.
        /// </summary>
        public IcalOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/calendar"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        /// <summary>
        /// Writes the response body.
        /// </summary>
        /// <param name="context">The formatter context associated with the call.</param>
        /// <param name="selectedEncoding">The <see cref="Encoding"/> that should be used to write the response.</param>
        /// <returns>A task which can write the response body.</returns>
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var httpContext = context.HttpContext;
            var buffer = new StringBuilder();

            using var strWriter = new StringWriter(buffer);
            
            var icsWriter = new IcsWriter(strWriter);
            var icsCalendar = IcsCalendar.CreateWithoutDefaults();

            icsCalendar.ProductId = new IcsProductId("-//STUEBER SYSTEMS//NONSGML OpenHolidaysApi//EN");
            icsCalendar.Method = new IcsMethod("PUBLISH");
            icsCalendar.Scale = new IcsScale("GREGORIAN");
            icsCalendar.Version = new IcsVersion("2.0");

            if (context.Object is IEnumerable<HolidayResponse> holidays)
            {
                foreach (var holiday in holidays)
                {
                    icsCalendar.EventList.Add(holiday.CreateIcsEvent());
                }
            }
            else
            {
                icsCalendar.EventList.Add((context.Object! as HolidayResponse).CreateIcsEvent());
            }

            await icsWriter.WriteAsync(icsCalendar);

            await httpContext.Response.WriteAsync(buffer.ToString(), selectedEncoding);
        }

        /// <summary>
        /// Returns a value indicating whether or not the given type can be written by this serializer.
        /// </summary>
        /// <param name="type">The object type.</param>
        /// <returns>TRUE if the type can be written, otherwise FALSE.</returns>
        protected override bool CanWriteType(Type type)
        {
            return typeof(HolidayResponse).IsAssignableFrom(type) || typeof(IEnumerable<HolidayResponse>).IsAssignableFrom(type);
        }
    }
}
 