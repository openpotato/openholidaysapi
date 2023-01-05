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
using Microsoft.EntityFrameworkCore;
using OpenHolidaysApi.DataLayer;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenHolidaysApi
{
    /// <summary>
    /// Representation of a holiday
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class HolidayResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HolidayResponse"/> class.
        /// </summary>
        /// <param name="holiday">Assigns data from <see cref="Holiday"/></param>
        /// <param name="languageCode">Language code or null</param>
        public HolidayResponse(Holiday holiday, string languageCode)
        {
            Id = holiday.Id;
            StartDate = holiday.StartDate;
            EndDate = holiday.EndDate;
            Type = holiday.Type;
            Details = holiday.Details;
            Nationwide = holiday.Nationwide;
            Subdivisions = holiday.Subdivisions.Select(x => new SubdivisionReference() { IsoCode = x.IsoCode, ShortName = x.ShortName }).ToList();
            OUnits = holiday.OUnits.Select(x => new OUnitReference() { Code = x.Code, ShortName = x.ShortName }).ToList();
            Names = holiday.Names.ToLocalizedList(languageCode);
            Comments = holiday.Comments.ToLocalizedList(languageCode);
        }

        /// <summary>
        /// Additional localized comments
        /// </summary>
        [JsonPropertyOrder(8)]
        public ICollection<LocalizedText> Comments { get; set; }

        /// <summary>
        /// End date of the holiday
        /// </summary>
        /// <example>2022-12-31</example>
        [Required]
        [JsonPropertyOrder(2)]
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// Unqiue holiday id
        /// </summary>
        /// <example>ff3b77a3-8c31-47af-b1c7-f26dd51f3c19</example>
        [Required]
        [JsonPropertyOrder(0)]
        public Guid Id { get; set; }

        /// <summary>
        /// Localized names of the holiday
        /// </summary>
        [Required]
        [JsonPropertyOrder(5)]
        public ICollection<LocalizedText> Names { get; set; }

        /// <summary>
        /// Additional detailed information
        /// </summary>
        [Required]
        [JsonPropertyOrder(4)]
        public HolidayDetails Details { get; set; }

        /// <summary>
        /// Is the holiday nationwide?
        /// </summary>
        /// <example>true</example>
        [Required]
        [JsonPropertyOrder(6)]
        public bool Nationwide { get; set; }

        /// <summary>
        /// Start date of the holiday
        /// </summary>
        /// <example>2022-01-01</example>
        [Required]
        [JsonPropertyOrder(1)]
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// List of organizational unit references
        /// </summary>
        [Required]
        [JsonPropertyOrder(8)]
        public ICollection<OUnitReference> OUnits { get; set; }

        /// <summary>
        /// List of subdivision references
        /// </summary>
        [Required]
        [JsonPropertyOrder(7)]
        public ICollection<SubdivisionReference> Subdivisions { get; set; }

        /// <summary>
        /// Type of holiday
        /// </summary>
        /// <example>Public</example>
        [Required]
        [JsonPropertyOrder(3)]
        public HolidayType Type { get; set; }

        /// <summary>
        /// Creates an iCal event from the instance
        /// </summary>
        /// <returns>An iCal event</returns>
        public virtual IcsEvent CreateIcsEvent()
        {
            var icsEvent = new IcsEvent
            {
                Uid = new IcsUid(Id.ToString("N")),
                Start = new IcsDateTimeStart(StartDate),
                End = new IcsDateTimeEnd(EndDate.AddDays(1)),
                Classification = new IcsClassification(IcsClassificationValue.Public),
                Created = new IcsCreated(DateTime.Now),
                LastModified = new IcsLastModified(DateTime.Now),
                DateTimeStamp = new IcsDateTimeStamp(DateTime.Now),
                Transparency = new IcsTransparency(IcsTransparencyType.Opaque),
            };

            icsEvent.AddCategories(new string[] { Type.ToString() });

            icsEvent.Summary = new IcsSummary
            {
                Value = Names.First().Text,
                Language = Names.First().Language
            };

            if (!Nationwide)
            {
                icsEvent.Summary.Value = $"{icsEvent.Summary.Value} ({string.Join(",", Subdivisions.Select(x => x.ShortName).ToList())})";
            }

            if (OUnits.Count > 0)
            {
                icsEvent.Summary.Value = $"{icsEvent.Summary.Value} [{string.Join(",", OUnits.Select(x => x.ShortName).ToList())}]";
            };

            if (Comments.Count > 0)
            {
                icsEvent.Summary.Value += '*';
                icsEvent.Description = new IcsDescription
                {
                    Value = '*' + Comments.First().Text,
                    Language = Comments.First().Language
                };
            };

            return icsEvent;
        }
    }
}