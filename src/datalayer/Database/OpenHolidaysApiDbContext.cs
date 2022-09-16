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

namespace OpenHolidaysApi.DataLayer
{
    public class OpenHolidaysApiDbContext : DbContext
    {
        public OpenHolidaysApiDbContext()
            : base()
        {
        }

        public OpenHolidaysApiDbContext(DbContextOptions<OpenHolidaysApiDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>();
            modelBuilder.Entity<Language>();
            modelBuilder.Entity<Subdivision>();
            modelBuilder.Entity<Holiday>();
            modelBuilder.Entity<OUnit>(x =>
            {
                x.HasOne(c => c.Parent)
                 .WithMany(c => c.Children)
                 .HasForeignKey(c => c.ParentId);
            });
        }
    }
}
