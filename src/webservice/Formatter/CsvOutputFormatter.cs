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
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace OpenHolidaysApi
{
    /// <summary>
    /// Writes <see cref="CountryResponse"/> instances , <see cref="LanguageResponse"/> instances, <see cref="SubdivisionResponse"/>  instances or 
    /// <see cref="HolidayResponse"/> instances formatted as CSV to the output stream.
    /// </summary>
    public class CsvOutputFormatter : TextOutputFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IcsOutputFormatter"/> class.
        /// </summary>
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
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

            var csvWriter = new CsvTableWriter(strWriter);
            csvWriter.SetFormats<DateOnly>("yyyy-MM-dd");
            csvWriter.SetTrueFalseString<bool>("true", "false");

            if (context.Object is IEnumerable<CountryResponse> countries)
            {
                await csvWriter.WriteHeadersAsync(
                    "IsoCode",
                    "Name",
                    "OfficialLanguages");

                foreach (var country in countries)
                {
                    csvWriter.SetValue("IsoCode", country.IsoCode);
                    csvWriter.SetValue("Name", country.Name.ToSingleText("EN"));
                    csvWriter.SetValue("OfficialLanguages", $"{string.Join(",", country.OfficialLanguages)}");
                    await csvWriter.WriteAsync();
                }
            }
            else if (context.Object is IEnumerable<LanguageResponse> languages)
            {
                await csvWriter.WriteHeadersAsync(
                    "IsoCode",
                    "Name");

                foreach (var language in languages)
                {
                    csvWriter.SetValue("IsoCode", language.IsoCode);
                    csvWriter.SetValue("Name", language.Name.ToSingleText("EN"));
                    await csvWriter.WriteAsync();
                }
            }
            else if (context.Object is IEnumerable<SubdivisionResponse> subdivisions)
            {
                await csvWriter.WriteHeadersAsync(
                    "Code",
                    "IsoCode",
                    "ShortName",
                    "Category",
                    "Name",
                    "OfficialLanguages");

                foreach (var subdivision in subdivisions)
                {
                    csvWriter.SetValue("Code", subdivision.Code);
                    csvWriter.SetValue("IsoCode", subdivision.IsoCode);
                    csvWriter.SetValue("ShortName", subdivision.ShortName);
                    csvWriter.SetValue("Category", subdivision.Category.ToSingleText("EN"));
                    csvWriter.SetValue("Name", subdivision.Name.ToSingleText("EN"));
                    csvWriter.SetValue("OfficialLanguages", $"{string.Join(",", subdivision.OfficialLanguages)}");
                    await csvWriter.WriteAsync();
                }
            }
            else if (context.Object is IEnumerable<HolidayResponse> holidays)
            {
                await csvWriter.WriteHeadersAsync(
                    "Id",
                    "StartDate",
                    "EndDate",
                    "Type",
                    "Name",
                    "Nationwide",
                    "Subdivisions",
                    "Comment");

                foreach (var holiday in holidays)
                {
                    csvWriter.SetValue("Id", holiday.Id);
                    csvWriter.SetValue("StartDate", holiday.StartDate);
                    csvWriter.SetValue("EndDate", holiday.EndDate);
                    csvWriter.SetValue("Type", holiday.Type.ToString());
                    csvWriter.SetValue("Name", holiday.Name.ToSingleText("EN"));
                    csvWriter.SetValue("Nationwide", holiday.Nationwide);
                    csvWriter.SetValue("Subdivisions", $"{string.Join(",", holiday.Subdivisions.Select(x => x.Code).ToList())}");
                    csvWriter.SetValue("Comment", holiday.Comment.ToSingleText("EN"));
                    await csvWriter.WriteAsync();
                }
            }

            await httpContext.Response.WriteAsync(buffer.ToString(), selectedEncoding);
        }

        /// <summary>
        /// Returns a value indicating whether or not the given type can be written by this serializer.
        /// </summary>
        /// <param name="type">The object type.</param>
        /// <returns>TRUE if the type can be written, otherwise FALSE.</returns>
        protected override bool CanWriteType(Type type)
        {
            return
                typeof(IEnumerable<CountryResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<LanguageResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<SubdivisionResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<HolidayResponse>).IsAssignableFrom(type);
        }
    }
}
 