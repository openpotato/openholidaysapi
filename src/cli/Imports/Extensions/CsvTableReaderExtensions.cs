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
using OpenHolidaysApi.DataLayer;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace OpenHolidaysApi.CLI
{
    /// <summary>
    /// Extension methods for <see cref="CsvTableReader"/>
    /// </summary>
    public static class CsvTableReaderExtensions
    {
        public static CsvCountry GetCountry(this CsvTableReader csvTableReader)
        {
            var country = new CsvCountry
            {
                IsoCode = csvTableReader.GetValue<string>(CsvColumns.IsoCode),
                OfficialLanguages = csvTableReader.GetValue<ICollection<string>>(CsvColumns.OfficialLanguages)
            };

            var i = 1;
            while (true)
            {
                var nameHeader = $"{CsvColumns.Name}.{i:000}";
                var languageHeader = $"{CsvColumns.Language}.{i:000}";

                if (csvTableReader.Headers.Contains(x => x == languageHeader))
                {
                    if (csvTableReader.Headers.Contains(x => x == nameHeader))
                    {
                        country.Name.Add(new CsvLocalizedText
                        (
                            csvTableReader.GetValue<string>(languageHeader),
                            csvTableReader.GetValue<string>(nameHeader)
                        ));
                    }
                }
                else
                {
                    break;
                }
                i++;
            }

            return country;
        }

        public static CsvGroup GetGroup(this CsvTableReader csvTableReader)
        {
            var group = new CsvGroup
            {
                Country = csvTableReader.GetValue<string>(CsvColumns.Country),
                Code = csvTableReader.GetValue<string>(CsvColumns.Code),
                ShortName = csvTableReader.GetValue<string>(CsvColumns.ShortName),
                Parent = csvTableReader.GetValueOrDefault<string>(CsvColumns.Parent)
            };

            var i = 1;
            while (true)
            {
                var nameHeader = $"{CsvColumns.Name}.{i:000}";
                var categoryHeader = $"{CsvColumns.Category}.{i:000}";
                var languageHeader = $"{CsvColumns.Language}.{i:000}";

                if (csvTableReader.Headers.Contains(x => x == languageHeader))
                {
                    if (csvTableReader.Headers.Contains(x => x == categoryHeader))
                    {
                        group.Category.Add(new CsvLocalizedText
                        (
                            csvTableReader.GetValue<string>(languageHeader),
                            csvTableReader.GetValue<string>(categoryHeader)
                        ));
                    }
                    if (csvTableReader.Headers.Contains(x => x == nameHeader))
                    {
                        group.Name.Add(new CsvLocalizedText
                        (
                            csvTableReader.GetValue<string>(languageHeader),
                            csvTableReader.GetValue<string>(nameHeader)
                        ));
                    }
                }
                else
                {
                    break;
                }
                i++;
            }

            return group;
        }

        public static CsvHoliday GetHoliday(this CsvTableReader csvTableReader)
        {
            var holiday = new CsvHoliday
            {
                Id = csvTableReader.GetValue<Guid>(CsvColumns.Id),
                Country = csvTableReader.GetValue<string>(CsvColumns.Country),
                StartDate = csvTableReader.GetValueOrDefault<DateOnly>(CsvColumns.StartDate),
                EndDate = csvTableReader.GetValueOrDefault<DateOnly>(CsvColumns.EndDate),
                Type = csvTableReader.GetValue<HolidayType>(CsvColumns.Type),
                RegionalScope = csvTableReader.GetValue<RegionalScope>(CsvColumns.RegionalScope),
                TemporalScope = csvTableReader.GetValueOrDefault<TemporalScope?>(CsvColumns.TemporalScope),
                Subdivisions = csvTableReader.GetValueOrDefault<ICollection<string>>(CsvColumns.Subdivisions),
                Groups = csvTableReader.GetValueOrDefault<ICollection<string>>(CsvColumns.Groups),
                Tags = csvTableReader.GetValueOrDefault<HolidayTags?>(CsvColumns.Tags)
            };

            var i = 1;
            while (true)
            {
                var nameHeader = $"{CsvColumns.Name}.{i:000}";
                var commentHeader = $"{CsvColumns.Comment}.{i:000}";
                var languageHeader = $"{CsvColumns.Language}.{i:000}";

                if (csvTableReader.Headers.Contains(x => x == languageHeader))
                {
                    if (csvTableReader.Headers.Contains(x => x == commentHeader))
                    {
                        holiday.Comment.Add(new CsvLocalizedText
                        (
                            csvTableReader.GetValue<string>(languageHeader),
                            csvTableReader.GetValue<string>(commentHeader)
                        ));
                    }
                    if (csvTableReader.Headers.Contains(x => x == nameHeader))
                    {
                        holiday.Name.Add(new CsvLocalizedText
                        (
                            csvTableReader.GetValue<string>(languageHeader),
                            csvTableReader.GetValue<string>(nameHeader)
                        ));
                    }
                }
                else
                {
                    break;
                }
                i++;
            }

            return holiday;
        }

        public static CsvLanguage GetLanguage(this CsvTableReader csvTableReader)
        {
            var language = new CsvLanguage
            {
                IsoCode = csvTableReader.GetValue<string>("IsoCode")
            };

            var i = 1;
            while (true)
            {
                var nameHeader = $"{CsvColumns.Name}.{i:000}";
                var languageHeader = $"{CsvColumns.Language}.{i:000}";

                if (csvTableReader.Headers.Contains(x => x == languageHeader))
                {
                    if (csvTableReader.Headers.Contains(x => x == nameHeader))
                    {
                        language.Name.Add(new CsvLocalizedText
                        (
                            csvTableReader.GetValue<string>(languageHeader),
                            csvTableReader.GetValue<string>(nameHeader)
                        ));
                    }
                }
                else
                {
                    break;
                }
                i++;
            }

            return language;
        }

        public static CsvSubdivision GetSubdivision(this CsvTableReader csvTableReader)
        {
            var subdivision = new CsvSubdivision
            {
                Country = csvTableReader.GetValue<string>(CsvColumns.Country),
                Code = csvTableReader.GetValue<string>(CsvColumns.Code),
                IsoCode = csvTableReader.GetValueOrDefault<string>(CsvColumns.IsoCode),
                ShortName = csvTableReader.GetValue<string>(CsvColumns.ShortName),
                OfficialLanguages = csvTableReader.GetValue<ICollection<string>>(CsvColumns.OfficialLanguages),
                Groups = csvTableReader.GetValueOrDefault<ICollection<string>>(CsvColumns.Groups),
                Parent = csvTableReader.GetValueOrDefault<string>(CsvColumns.Parent)
            };

            var i = 1;
            while (true)
            {
                var nameHeader = $"{CsvColumns.Name}.{i:000}";
                var categoryHeader = $"{CsvColumns.Category}..{i:000}";
                var languageHeader = $"{CsvColumns.Language}.{i:000}";

                if (csvTableReader.Headers.Contains(x => x == languageHeader))
                {
                    if (csvTableReader.Headers.Contains(x => x == categoryHeader))
                    {
                        subdivision.Category.Add(new CsvLocalizedText
                        (
                            csvTableReader.GetValue<string>(languageHeader),
                            csvTableReader.GetValue<string>(categoryHeader)
                        ));
                    }
                    if (csvTableReader.Headers.Contains(x => x == nameHeader))
                    {
                        subdivision.Name.Add(new CsvLocalizedText
                        (
                            csvTableReader.GetValue<string>(languageHeader),
                            csvTableReader.GetValue<string>(nameHeader)
                        ));
                    }
                }
                else
                {
                    break;
                }
                i++;
            }

            return subdivision;
        }
        public static async IAsyncEnumerable<T> ReadAllAsync<T>(this CsvTableReader csvTableReader, Func<CsvTableReader, T> readAction,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            while (await csvTableReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false) > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return readAction(csvTableReader);
            }
        }
    }
}
