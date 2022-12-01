#region OpenHolidays API - Copyright (C) 2022 STÜBER SYSTEMS GmbH
/*    
 *    OpenHolidays API 
 *    
 *    Copyright (C) 2022 STÜBER SYSTEMS GmbH
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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OpenHolidaysApi.DataLayer
{
    /// <summary>
    ///  A factory for creating derived <see cref="DbContext" /> instances for EF design-time tools.
    /// </summary>
    /// <remarks>
    /// Please note that you have to use `remove-migration` with the `-Force` option, as there is no database 
    /// connection available.
    /// </remarks>
    class OpenHolidaysApiDbContextDesignTimeFactory : IDesignTimeDbContextFactory<OpenHolidaysApiDbContext>
    {
        public OpenHolidaysApiDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OpenHolidaysApiDbContext>();

            // No connection string
            optionsBuilder.UseNpgsql();

            return new OpenHolidaysApiDbContext(optionsBuilder.Options);
        }
    }
}
