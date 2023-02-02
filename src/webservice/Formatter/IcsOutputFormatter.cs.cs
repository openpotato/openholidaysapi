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

using Enbrea.Ics;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace OpenHolidaysApi
{
    /// <summary>
    /// Writes <see cref="HolidayResponse"/> instances formatted as iCalendar to the output stream.
    /// </summary>
    public class IcsOutputFormatter : TextOutputFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IcsOutputFormatter"/> class.
        /// </summary>
        public IcsOutputFormatter()
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
                    var icsEvent = new IcsEvent
                    {
                        Uid = new IcsUid(holiday.Id.ToString("N")),
                        Start = new IcsDateTimeStart(holiday.StartDate),
                        End = new IcsDateTimeEnd(holiday.EndDate.AddDays(1)),
                        Classification = new IcsClassification(IcsClassificationValue.Public),
                        Created = new IcsCreated(DateTime.Now),
                        LastModified = new IcsLastModified(DateTime.Now),
                        DateTimeStamp = new IcsDateTimeStamp(DateTime.Now),
                        Transparency = new IcsTransparency(IcsTransparencyType.Opaque),
                    };

                    icsEvent.AddCategories(new string[] { holiday.Type.ToString() });

                    icsEvent.Summary = new IcsSummary
                    {
                        Value = holiday.Name.ToSingleText(null),
                        Language = holiday.Name.ToSingleLanguage(null),
                    };

                    if (!holiday.Nationwide)
                    {
                        icsEvent.Summary.Value = $"{icsEvent.Summary.Value} ({string.Join(",", holiday.Subdivisions.Select(x => x.ShortName).ToList())})";
                    }

                    if (holiday.Comment.Count > 0)
                    {
                        icsEvent.Summary.Value += '*';
                        icsEvent.Description = new IcsDescription
                        {
                            Value = '*' + holiday.Comment.ToSingleText(null),
                            Language = holiday.Comment.ToSingleLanguage(null)
                        };
                    };

                    icsCalendar.EventList.Add(icsEvent);
                }
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
 