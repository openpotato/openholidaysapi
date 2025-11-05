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

using Microsoft.EntityFrameworkCore;

namespace OpenHolidaysApi.DataLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
            : base()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>();
            modelBuilder.Entity<Language>();
            modelBuilder.Entity<Subdivision>(x =>
            {
                x.HasOne(c => c.Parent)
                 .WithMany(c => c.Children)
                 .HasForeignKey(c => c.ParentId);
                x.HasMany(left => left.Groups)
                 .WithMany(right => right.Subdivisions)
                 .UsingEntity(join => join.ToTable(DbTables.SubdivisionGroup, t => t.HasComment("Join table between Subdivision and Group")));
                x.HasMany(left => left.Holidays)
                 .WithMany(right => right.Subdivisions)
                 .UsingEntity(join => join.ToTable(DbTables.SubdivisionHoliday, t => t.HasComment("Join table between Subdivision and Holiday")));
            });
            modelBuilder.Entity<Group>(x =>
            {
                x.HasOne(c => c.Parent)
                 .WithMany(c => c.Children)
                 .HasForeignKey(c => c.ParentId);
                x.HasMany(left => left.Subdivisions)
                 .WithMany(right => right.Groups)
                 .UsingEntity(join => join.ToTable(DbTables.SubdivisionGroup, t => t.HasComment("Join table between Subdivision and Group")));
                x.HasMany(left => left.Holidays)
                 .WithMany(right => right.Groups)
                 .UsingEntity(join => join.ToTable(DbTables.GroupHoliday, t => t.HasComment("Join table between Group and Holiday")));
            });
            modelBuilder.Entity<Holiday>();
        }
    }
}
