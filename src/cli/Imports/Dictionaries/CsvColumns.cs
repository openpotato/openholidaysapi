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

namespace OpenHolidaysApi.DataLayer
{
    /// <summary>
    /// CSV field names for import
    /// </summary>
    public static class CsvColumns
    {
        public const string Category = "Category";
        public const string Code = "Code";
        public const string Comment = "Comment";
        public const string Country = "Country";
        public const string EndDate = "EndDate";
        public const string Groups = "Groups";
        public const string Id = "Id";
        public const string IsoCode = "IsoCode";
        public const string Language = "Language";
        public const string Name = "Name";
        public const string OfficialLanguages = "OfficialLanguages";
        public const string Parent = "Parent";
        public const string RegionalScope = "RegionalScope";
        public const string ShortName = "ShortName";
        public const string StartDate = "StartDate";
        public const string Subdivisions = "Subdivisions";
        public const string Tags = "Tags";
        public const string TemporalScope = "TemporalScope";
        public const string Type = "Type";
    }
}