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
    class OpenHolidaysApiDbContextDesignTimeFactory : IDesignTimeDbContextFactory<OpenHolidaysApiDbContext>
    {
        public OpenHolidaysApiDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OpenHolidaysApiDbContext>();

            /* 
             * dummy connection string
             * add-migration checks that DBProvider exists and has not empty connection string
             * remove-migration fails while checking in the db already applied migrations. use with -Force option
             */
            optionsBuilder.UseNpgsql();

            return new OpenHolidaysApiDbContext(optionsBuilder.Options);
        }
    }
}
